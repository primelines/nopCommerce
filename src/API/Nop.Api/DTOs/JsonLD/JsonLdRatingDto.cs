using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdRating")]
public record JsonLdRatingDto : JsonLdDto
{
    #region Properties


    [JsonProperty("@type")]
    public static string Type => "Rating";

    [JsonProperty("best_rating")]
    public string BestRating { get; set; }

    [JsonProperty("rating_value")]
    public int RatingValue { get; set; }

    [JsonProperty("worst_rating")]
    public string WorstRating { get; set; }

    #endregion
}
