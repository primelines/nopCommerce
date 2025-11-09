using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ActiveDiscussions")]
public partial record ActiveDiscussionsDto : BaseNopDto
{
    public ActiveDiscussionsDto()
    {
        ForumTopics = new List<ForumTopicRowDto>();
    }

    public IList<ForumTopicRowDto> ForumTopics { get; protected set; }


    [JsonProperty("view_all_link_enabled")]
    public bool ViewAllLinkEnabled { get; set; }


    [JsonProperty("active_discussions_feed_enabled")]
    public bool ActiveDiscussionsFeedEnabled { get; set; }


    [JsonProperty("topic_page_size")]
    public int TopicPageSize { get; set; }

    [JsonProperty("topic_total_records")]
    public int TopicTotalRecords { get; set; }

    [JsonProperty("topic_page_index")]
    public int TopicPageIndex { get; set; }


    [JsonProperty("posts_page_size")]
    public int PostsPageSize { get; set; }


    [JsonProperty("allow_post_voting")]
    public bool AllowPostVoting { get; set; }
}
