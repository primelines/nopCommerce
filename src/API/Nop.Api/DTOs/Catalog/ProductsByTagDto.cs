using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductsByTag")]
public partial record ProductsByTagDto : BaseNopEntityDto
{
    public ProductsByTagDto()
    {
        CatalogProducts = new CatalogProductsDto();
    }


    [JsonProperty("tag_name")]
    public string TagName { get; set; }

    [JsonProperty("tag_se_name")]
    public string TagSeName { get; set; }


    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }
}
