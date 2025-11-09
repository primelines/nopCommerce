using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.UI.Paging;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a catalog products model
/// </summary>
[JsonObject(Title = "CatalogProducts")]
public partial record CatalogProductsDto : BasePageableDto
{
    #region Properties

    /// <summary>
    /// Get or set a value indicating whether to use standard or AJAX products loading (applicable to 'paging', 'filtering', 'view modes') in catalog
    /// </summary>

    [JsonProperty("use_ajax_loading")]
    public bool UseAjaxLoading { get; set; }

    /// <summary>
    /// Gets or sets the warning message
    /// </summary>

    [JsonProperty("warning_message")]
    public string WarningMessage { get; set; }

    /// <summary>
    /// Gets or sets the message if there are no products to return
    /// </summary>

    [JsonProperty("no_result_message")]
    public string NoResultMessage { get; set; }

    /// <summary>
    /// Gets or sets the price range filter model
    /// </summary>

    [JsonProperty("price_range_filter")]
    public PriceRangeFilterDto PriceRangeFilter { get; set; }

    /// <summary>
    /// Gets or sets the specification filter model
    /// </summary>

    [JsonProperty("specification_filter")]
    public SpecificationFilterDto SpecificationFilter { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer filter model
    /// </summary>

    [JsonProperty("manufacturer_filter")]
    public ManufacturerFilterDto ManufacturerFilter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether product sorting is allowed
    /// </summary>

    [JsonProperty("allow_product_sorting")]
    public bool AllowProductSorting { get; set; }

    /// <summary>
    /// Gets or sets available sort options
    /// </summary>

    [JsonProperty("available_sort_options")]
    public IList<SelectListItemDto> AvailableSortOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether customers are allowed to change view mode
    /// </summary>

    [JsonProperty("allow_product_view_mode_changing")]
    public bool AllowProductViewModeChanging { get; set; }

    /// <summary>
    /// Gets or sets available view mode options
    /// </summary>

    [JsonProperty("available_view_modes")]
    public IList<SelectListItemDto> AvailableViewModes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether customers are allowed to select page size
    /// </summary>

    [JsonProperty("allow_customers_to_select_page_size")]
    public bool AllowCustomersToSelectPageSize { get; set; }

    /// <summary>
    /// Gets or sets available page size options
    /// </summary>

    [JsonProperty("page_size_options")]
    public IList<SelectListItemDto> PageSizeOptions { get; set; }

    /// <summary>
    /// Gets or sets a order by
    /// </summary>

    [JsonProperty("order_by")]
    public int? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets a product sorting
    /// </summary>

    [JsonProperty("view_mode")]
    public string ViewMode { get; set; }

    /// <summary>
    /// Gets or sets the products
    /// </summary>

    [JsonProperty("products")]
    public IList<ProductOverviewDto> Products { get; set; }

    #endregion

    #region Ctor

    public CatalogProductsDto()
    {
        PriceRangeFilter = new PriceRangeFilterDto();
        SpecificationFilter = new SpecificationFilterDto();
        ManufacturerFilter = new ManufacturerFilterDto();
        AvailableSortOptions = new List<SelectListItemDto>();
        AvailableViewModes = new List<SelectListItemDto>();
        PageSizeOptions = new List<SelectListItemDto>();
        Products = new List<ProductOverviewDto>();
    }

    #endregion
}
