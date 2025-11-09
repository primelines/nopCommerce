using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdAggregateRating")]
public record JsonLdAggregateRatingDto : JsonLdDto
{
    #region Properties


    [JsonProperty("@type")]
    public static string Type => "AggregateRating";

    [JsonProperty("rating_value")]
    public string RatingValue { get; set; }

    [JsonProperty("review_count")]
    public int ReviewCount { get; set; }

    #endregion
}
