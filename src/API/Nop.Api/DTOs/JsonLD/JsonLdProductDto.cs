using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdProduct")]
public record JsonLdProductDto : JsonLdDto
{
    #region Ctor

    public JsonLdProductDto()
    {
        Brand = new List<JsonLdBrandDto>();
        Review = new List<JsonLdReviewDto>();
        HasVariant = new List<JsonLdProductDto>();
    }

    #endregion

    #region Properties


    [JsonProperty("@context")]
    public static string Context => "https://schema.org";


    [JsonProperty("@type")]
    public static string Type => "Product";


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("gtin")]
    public string Gtin { get; set; }

    [JsonProperty("mpn")]
    public string Mpn { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("brand")]
    public IList<JsonLdBrandDto> Brand { get; set; }

    [JsonProperty("offer")]
    public JsonLdOfferDto Offer { get; set; }

    [JsonProperty("aggregate_rating")]
    public JsonLdAggregateRatingDto AggregateRating { get; set; }

    [JsonProperty("review")]
    public IList<JsonLdReviewDto> Review { get; set; }

    [JsonProperty("has_variant")]
    public IList<JsonLdProductDto> HasVariant { get; set; }

    #endregion
}
