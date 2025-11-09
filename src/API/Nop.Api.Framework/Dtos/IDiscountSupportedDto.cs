using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents a discount supported model
/// </summary>
public partial interface IDiscountSupportedDto
{
    #region Properties

    /// <summary>
    /// Gets or sets identifiers of the selected discounts
    /// </summary>
    IList<int> SelectedDiscountIds { get; set; }

    /// <summary>
    /// Gets or sets items for the all available discounts
    /// </summary>
    IList<SelectListItem> AvailableDiscounts { get; set; }

    #endregion
}