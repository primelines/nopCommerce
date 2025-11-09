using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductReviews")]
public partial record ProductReviewsDto : BaseNopDto
{
    public ProductReviewsDto()
    {
        Items = new List<ProductReviewDto>();
        AddProductReview = new AddProductReviewDto();
        ReviewTypeList = new List<ReviewTypeDto>();
        AddAdditionalProductReviewList = new List<AddProductReviewReviewTypeMappingDto>();
    }


    [JsonProperty("product_id")]
    public int ProductId { get; set; }


    [JsonProperty("items")]
    public IList<ProductReviewDto> Items { get; set; }


    [JsonProperty("add_product_review")]
    public AddProductReviewDto AddProductReview { get; set; }


    [JsonProperty("review_type_list")]
    public IList<ReviewTypeDto> ReviewTypeList { get; set; }


    [JsonProperty("add_additional_product_review_list")]
    public IList<AddProductReviewReviewTypeMappingDto> AddAdditionalProductReviewList { get; set; }
}
