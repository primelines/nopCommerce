using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "AddProductReview")]
public partial record AddProductReviewDto : BaseNopDto
{

    [JsonProperty("title")]
    public string Title { get; set; }


    [JsonProperty("review_text")]
    public string ReviewText { get; set; }


    [JsonProperty("rating")]
    public int Rating { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }


    [JsonProperty("can_current_customer_leave_review")]
    public bool CanCurrentCustomerLeaveReview { get; set; }


    [JsonProperty("can_add_new_review")]
    public bool CanAddNewReview { get; set; }
}
