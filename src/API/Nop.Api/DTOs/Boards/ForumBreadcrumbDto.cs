using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumBreadcrumb")]
public partial record ForumBreadcrumbDto : BaseNopDto
{

    [JsonProperty("forum_group_id")]
    public int ForumGroupId { get; set; }

    [JsonProperty("forum_group_name")]
    public string ForumGroupName { get; set; }

    [JsonProperty("forum_group_se_name")]
    public string ForumGroupSeName { get; set; }


    [JsonProperty("forum_id")]
    public int ForumId { get; set; }

    [JsonProperty("forum_name")]
    public string ForumName { get; set; }

    [JsonProperty("forum_se_name")]
    public string ForumSeName { get; set; }


    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }

    [JsonProperty("forum_topic_subject")]
    public string ForumTopicSubject { get; set; }

    [JsonProperty("forum_topic_se_name")]
    public string ForumTopicSeName { get; set; }
}
