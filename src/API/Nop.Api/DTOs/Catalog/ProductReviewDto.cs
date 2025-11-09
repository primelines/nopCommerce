using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductReview")]
public partial record ProductReviewDto : BaseNopEntityDto
{
    public ProductReviewDto()
    {
        AdditionalProductReviewList = new List<ProductReviewReviewTypeMappingDto>();
    }


    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }


    [JsonProperty("customer_avatar_url")]
    public string CustomerAvatarUrl { get; set; }


    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }


    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }


    [JsonProperty("title")]
    public string Title { get; set; }


    [JsonProperty("review_text")]
    public string ReviewText { get; set; }


    [JsonProperty("reply_text")]
    public string ReplyText { get; set; }


    [JsonProperty("rating")]
    public int Rating { get; set; }


    [JsonProperty("written_on_str")]
    public string WrittenOnStr { get; set; }


    [JsonProperty("helpfulness")]
    public ProductReviewHelpfulnessModel Helpfulness { get; set; }


    [JsonProperty("additional_product_review_list")]
    public IList<ProductReviewReviewTypeMappingDto> AdditionalProductReviewList { get; set; }
}
