using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumSubscription")]
public partial record ForumSubscriptionDto : BaseNopEntityDto
{

    [JsonProperty("forum_id")]
    public int ForumId { get; set; }

    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }

    [JsonProperty("topic_subscription")]
    public bool TopicSubscription { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }
}
