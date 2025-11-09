using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "AddProductReviewReviewTypeMapping")]
public partial record AddProductReviewReviewTypeMappingDto : BaseNopEntityDto
{

    [JsonProperty("product_review_id")]
    public int ProductReviewId { get; set; }


    [JsonProperty("review_type_id")]
    public int ReviewTypeId { get; set; }


    [JsonProperty("rating")]
    public int Rating { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("display_order")]
    public int DisplayOrder { get; set; }


    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }
}
