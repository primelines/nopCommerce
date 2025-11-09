using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdReview")]
public record JsonLdReviewDto : JsonLdDto
{
    #region Properties


    [JsonProperty("@type")]
    public static string Type => "Review";

    [JsonProperty("author")]
    public JsonLdPersonDto Author { get; set; }


    [JsonProperty("date_published")]
    public string DatePublished { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("review_body")]
    public string ReviewBody { get; set; }

    [JsonProperty("review_rating")]
    public JsonLdRatingDto ReviewRating { get; set; }

    #endregion
}
