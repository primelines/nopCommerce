using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Social")]
public partial record SocialDto : BaseNopDto
{

    [JsonProperty("facebook_link")]
    public string FacebookLink { get; set; }

    [JsonProperty("twitter_link")]
    public string TwitterLink { get; set; }

    [JsonProperty("youtube_link")]
    public string YoutubeLink { get; set; }

    [JsonProperty("instagram_link")]
    public string InstagramLink { get; set; }

    [JsonProperty("working_language_id")]
    public int WorkingLanguageId { get; set; }

    [JsonProperty("news_enabled")]
    public bool NewsEnabled { get; set; }
}
