using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Profile;

[JsonObject(Title = "Posts")]
public partial record PostsDto : BaseNopDto
{

    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }

    [JsonProperty("forum_topic_title")]
    public string ForumTopicTitle { get; set; }

    [JsonProperty("forum_topic_slug")]
    public string ForumTopicSlug { get; set; }

    [JsonProperty("forum_post_text")]
    public string ForumPostText { get; set; }

    [JsonProperty("posted")]
    public string Posted { get; set; }
}
