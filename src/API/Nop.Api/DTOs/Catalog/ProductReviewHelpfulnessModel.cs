using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductReviewHelpfulness")]
public partial record ProductReviewHelpfulnessModel : BaseNopDto
{

    [JsonProperty("product_review_id")]
    public int ProductReviewId { get; set; }


    [JsonProperty("helpful_yes_total")]
    public int HelpfulYesTotal { get; set; }


    [JsonProperty("helpful_no_total")]
    public int HelpfulNoTotal { get; set; }
}
