using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Infrastructure;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CustomerProductReview")]
public partial record CustomerProductReviewDto : BaseNopDto
{
    public CustomerProductReviewDto()
    {
        AdditionalProductReviewList = new List<ProductReviewReviewTypeMappingDto>();
    }

    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
    public string ProductSeName { get; set; }

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

    [JsonProperty("approval_status")]
    public string ApprovalStatus { get; set; }

    [JsonProperty("additional_product_review_list")]
    public IList<ProductReviewReviewTypeMappingDto> AdditionalProductReviewList { get; set; }
}

[JsonObject(Title = "CustomerProductReviews")]
public partial record CustomerProductReviewsDto : BaseNopDto
{
    public CustomerProductReviewsDto()
    {
        ProductReviews = new List<CustomerProductReviewDto>();
    }


    [JsonProperty("product_reviews")]
    public IList<CustomerProductReviewDto> ProductReviews { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }

    #region Nested class

    /// <summary>
    /// record that has only page for route value. Used for (My Account) My Product Reviews pagination
    /// </summary>
    public partial record CustomerProductReviewsRouteValues : IRouteValues
    {

    [JsonProperty("page_number")]
        public int PageNumber { get; set; }
    }

    #endregion
}
