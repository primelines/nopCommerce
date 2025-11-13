using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductReviewOverview")]
public partial record ProductReviewOverviewDto : BaseNopDto
{

    [JsonProperty("product_id")]
    public int ProductId { get; set; }


    [JsonProperty("rating_sum")]
    public int RatingSum { get; set; }


    [JsonProperty("total_reviews")]
    public int TotalReviews { get; set; }

    [JsonProperty("can_add_new_review")]
    public bool CanAddNewReview { get; set; }

    [JsonProperty("can_current_customer_leave_review")]
    public bool CanCurrentCustomerLeaveReview { get; set; }
}
