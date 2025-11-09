using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "LastPost")]
public partial record LastPostDto : BaseNopDto
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }

    [JsonProperty("forum_topic_se_name")]
    public string ForumTopicSeName { get; set; }

    [JsonProperty("forum_topic_subject")]
    public string ForumTopicSubject { get; set; }


    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }

    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }

    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }


    [JsonProperty("post_created_on_str")]
    public string PostCreatedOnStr { get; set; }


    [JsonProperty("show_topic")]
    public bool ShowTopic { get; set; }
}
