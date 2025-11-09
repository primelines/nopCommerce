using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Discounts;
using Nop.Services.Discounts;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.Framework.Factories;

/// <summary>
/// Represents the base discount supported model factory implementation
/// </summary>
public partial class DiscountSupportedDtoFactory : IDiscountSupportedDtoFactory
{
    #region Fields

    protected readonly IDiscountService _discountService;

    #endregion

    #region Ctor

    public DiscountSupportedDtoFactory(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare selected and all available discounts for the passed model
    /// </summary>
    /// <typeparam name="TModel">Discount supported model type</typeparam>
    /// <param name="model">Model</param>
    /// <param name="availableDiscounts">List of all available discounts</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task<TModel> PrepareModelDiscountsAsync<TModel>(TModel model, IList<Discount> availableDiscounts) where TModel : IDiscountSupportedDto
    {
        ArgumentNullException.ThrowIfNull(model);

        //prepare available discounts
        model.AvailableDiscounts = availableDiscounts.Select(discount => new SelectListItem
        {
            Text = discount.Name,
            Value = discount.Id.ToString(),
            Selected = model.SelectedDiscountIds.Contains(discount.Id)
        }).ToList();

        return Task.FromResult(model);
    }

    /// <summary>
    /// Prepare selected and all available discounts for the passed model by entity applied discounts
    /// </summary>
    /// <typeparam name="TModel">Discount supported model type</typeparam>
    /// <typeparam name="TMapping">Discount supported entity type</typeparam>
    /// <param name="model">Model</param>
    /// <param name="entity">Entity</param>
    /// <param name="availableDiscounts">List of all available discounts</param>
    /// <param name="ignoreAppliedDiscounts">Whether to ignore existing applied discounts</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<TModel> PrepareModelDiscountsAsync<TModel, TMapping>(TModel model, IDiscountSupported<TMapping> entity,
        IList<Discount> availableDiscounts, bool ignoreAppliedDiscounts)
        where TModel : IDiscountSupportedDto where TMapping : DiscountMapping
    {
        ArgumentNullException.ThrowIfNull(model);

        //prepare already applied discounts
        if (!ignoreAppliedDiscounts && entity != null)
            model.SelectedDiscountIds = (await _discountService.GetAppliedDiscountsAsync(entity)).Select(discount => discount.Id).ToList();

        return await PrepareModelDiscountsAsync(model, availableDiscounts);
    }

    #endregion
}