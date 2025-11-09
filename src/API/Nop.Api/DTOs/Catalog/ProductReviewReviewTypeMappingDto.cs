using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductReviewReviewTypeMapping")]
public partial record ProductReviewReviewTypeMappingDto : BaseNopEntityDto
{

    [JsonProperty("product_review_id")]
    public int ProductReviewId { get; set; }


    [JsonProperty("review_type_id")]
    public int ReviewTypeId { get; set; }


    [JsonProperty("rating")]
    public int Rating { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("visible_to_all_customers")]
    public bool VisibleToAllCustomers { get; set; }
}
