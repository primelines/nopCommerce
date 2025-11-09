using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductOverview")]
public partial record ProductOverviewDto : BaseNopEntityDto
{
    public ProductOverviewDto()
    {
        ProductPrice = new ProductPriceOverviewDto();
        Pictures = new List<PictureDto>();
        ProductSpecification = new ProductSpecificationDto();
        ReviewOverview = new ProductReviewOverviewDto();
    }


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; }

    [JsonProperty("full_description")]
    public string FullDescription { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("sku")]
    public string Sku { get; set; }


    [JsonProperty("product_type")]
    public ProductType ProductType { get; set; }


    [JsonProperty("mark_as_new")]
    public bool MarkAsNew { get; set; }

    //price

    [JsonProperty("product_price")]
    public ProductPriceOverviewDto ProductPrice { get; set; }
    //pictures

    [JsonProperty("pictures")]
    public IList<PictureDto> Pictures { get; set; }
    //specification attributes

    [JsonProperty("product_specification")]
    public ProductSpecificationDto ProductSpecification { get; set; }
    //price

    [JsonProperty("review_overview")]
    public ProductReviewOverviewDto ReviewOverview { get; set; }
}
