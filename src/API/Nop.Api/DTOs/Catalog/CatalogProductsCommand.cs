using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Nop.Api.Framework.UI.Paging;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a model to get the catalog products
/// </summary>
[JsonObject(Title = "CatalogProductsCommand")]
public partial record CatalogProductsCommand : BasePageableDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the price ('min-max' format)
    /// </summary>

    [JsonProperty("price")]
    public string Price { get; set; }

    /// <summary>
    /// Gets or sets the specification attribute option ids
    /// </summary>
    [FromQuery(Name = "specs")]

    [JsonProperty("specification_option_ids")]
    public List<int> SpecificationOptionIds { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer ids
    /// </summary>
    [FromQuery(Name = "ms")]

    [JsonProperty("manufacturer_ids")]
    public List<int> ManufacturerIds { get; set; }

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

    #endregion
}
