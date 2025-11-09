using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CatalogSearch")]
public partial record CatalogSearchDto : BaseNopDto
{
    public CatalogSearchDto()
    {
        AvailableCategories = new List<SelectListItemDto>();
        AvailableManufacturers = new List<SelectListItemDto>();
        AvailableVendors = new List<SelectListItemDto>();
        CatalogProducts = new CatalogProductsDto();
    }

    /// <summary>
    /// Query string
    /// </summary>

    [JsonProperty("query_string")]
    public string q { get; set; }

    /// <summary>
    /// Category ID
    /// </summary>

    [JsonProperty("category_id")]
    public int cid { get; set; }


    [JsonProperty("isc")]
    public bool isc { get; set; }

    /// <summary>
    /// Manufacturer ID
    /// </summary>

    [JsonProperty("manufacturer_id")]
    public int mid { get; set; }

    /// <summary>
    /// Vendor ID
    /// </summary>

    [JsonProperty("vendor_id")]
    public int vid { get; set; }

    /// <summary>
    /// A value indicating whether to search in descriptions
    /// </summary>

    [JsonProperty("search_in_descriptions")]
    public bool sid { get; set; }

    /// <summary>
    /// A value indicating whether "advanced search" is enabled
    /// </summary>

    [JsonProperty("is_advanced_search")]
    public bool advs { get; set; }

    /// <summary>
    /// A value indicating whether "allow search by vendor" is enabled
    /// </summary>

    [JsonProperty("allow_search_by_vendor")]
    public bool asv { get; set; }


    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }


    [JsonProperty("available_categories")]
    public IList<SelectListItemDto> AvailableCategories { get; set; }

    [JsonProperty("available_manufacturers")]
    public IList<SelectListItemDto> AvailableManufacturers { get; set; }

    [JsonProperty("available_vendors")]
    public IList<SelectListItemDto> AvailableVendors { get; set; }
}
