using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "Manufacturer")]
public partial record ManufacturerDto : BaseNopEntityDto
{
    public ManufacturerDto()
    {
        Picture = new PictureDto();
        FeaturedProducts = new List<ProductOverviewDto>();
        CatalogProducts = new CatalogProductsDto();
    }


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("meta_keywords")]
    public string MetaKeywords { get; set; }

    [JsonProperty("meta_description")]
    public string MetaDescription { get; set; }

    [JsonProperty("meta_title")]
    public string MetaTitle { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("picture")]
    public PictureDto Picture { get; set; }


    [JsonProperty("featured_products")]
    public IList<ProductOverviewDto> FeaturedProducts { get; set; }


    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }
}
