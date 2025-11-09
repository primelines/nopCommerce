using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a new products model
/// </summary>
[JsonObject(Title = "NewProducts")]
public partial record NewProductsDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the catalog products model
    /// </summary>

    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }

    #endregion

    #region Ctor

    public NewProductsDto()
    {
        CatalogProducts = new CatalogProductsDto();
    }

    #endregion
}
