using Nop.Core.Domain.Orders;
using Nop.Api.DTOs.Media;
using Nop.Api.DTOs.ShoppingCart;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the interface of the shopping cart model factory
/// </summary>
public partial interface IShoppingCartDtoFactory
{
    /// <summary>
    /// Prepare the estimate shipping model
    /// </summary>
    /// <param name="cart">List of the shopping cart item</param>
    /// <param name="setEstimateShippingDefaultAddress">Whether to use customer default shipping address for estimating</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the estimate shipping model
    /// </returns>
    Task<EstimateShippingDto> PrepareEstimateShippingDtoAsync(IList<ShoppingCartItem> cart, bool setEstimateShippingDefaultAddress = true);

    /// <summary>
    /// Prepare the shopping cart model
    /// </summary>
    /// <param name="model">Shopping cart model</param>
    /// <param name="cart">List of the shopping cart item</param>
    /// <param name="isEditable">Whether model is editable</param>
    /// <param name="validateCheckoutAttributes">Whether to validate checkout attributes</param>
    /// <param name="prepareAndDisplayOrderReviewData">Whether to prepare and display order review data</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the shopping cart model
    /// </returns>
    Task<ShoppingCartDto> PrepareShoppingCartDtoAsync(ShoppingCartDto model,
        IList<ShoppingCartItem> cart, bool isEditable = true,
        bool validateCheckoutAttributes = false,
        bool prepareAndDisplayOrderReviewData = false);

    /// <summary>
    /// Prepare the mini shopping cart model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the mini shopping cart model
    /// </returns>
    Task<MiniShoppingCartDto> PrepareMiniShoppingCartDtoAsync();

    /// <summary>
    /// Prepare selected checkout attributes
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the formatted attributes
    /// </returns>
    Task<string> FormatSelectedCheckoutAttributesAsync();

    /// <summary>
    /// Prepare the order totals model
    /// </summary>
    /// <param name="cart">List of the shopping cart item</param>
    /// <param name="isEditable">Whether model is editable</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the order totals model
    /// </returns>
    Task<OrderTotalsDto> PrepareOrderTotalsDtoAsync(IList<ShoppingCartItem> cart, bool isEditable);

    /// <summary>
    /// Prepare the estimate shipping result model
    /// </summary>
    /// <param name="cart">List of the shopping cart item</param>
    /// <param name="request">Request to get shipping options</param>
    /// <param name="cacheOfferedShippingOptions">Indicates whether to cache offered shipping options</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the estimate shipping result model
    /// </returns>
    Task<EstimateShippingResultDto> PrepareEstimateShippingResultModelAsync(IList<ShoppingCartItem> cart, EstimateShippingDto request, bool cacheOfferedShippingOptions);


    /// <summary>
    /// Prepare the cart item picture model
    /// </summary>
    /// <param name="sci">Shopping cart item</param>
    /// <param name="pictureSize">Picture size</param>
    /// <param name="showDefaultPicture">Whether to show the default picture</param>
    /// <param name="productName">Product name</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the picture model
    /// </returns>
    Task<PictureDto> PrepareCartItemPictureDtoAsync(ShoppingCartItem sci, int pictureSize, bool showDefaultPicture, string productName);
}