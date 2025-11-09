using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumPage")]
public partial record ForumPageDto : BaseNopDto
{
    public ForumPageDto()
    {
        ForumTopics = new List<ForumTopicRowDto>();
    }


    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("watch_forum_text")]
    public string WatchForumText { get; set; }


    [JsonProperty("forum_topics")]
    public IList<ForumTopicRowDto> ForumTopics { get; set; }

    [JsonProperty("topic_page_size")]
    public int TopicPageSize { get; set; }

    [JsonProperty("topic_total_records")]
    public int TopicTotalRecords { get; set; }

    [JsonProperty("topic_page_index")]
    public int TopicPageIndex { get; set; }


    [JsonProperty("is_customer_allowed_to_subscribe")]
    public bool IsCustomerAllowedToSubscribe { get; set; }


    [JsonProperty("forum_feeds_enabled")]
    public bool ForumFeedsEnabled { get; set; }


    [JsonProperty("posts_page_size")]
    public int PostsPageSize { get; set; }


    [JsonProperty("allow_post_voting")]
    public bool AllowPostVoting { get; set; }
}
