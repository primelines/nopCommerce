using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumTopicPage")]
public partial record ForumTopicPageDto : BaseNopDto
{
    public ForumTopicPageDto()
    {
        ForumPosts = new List<ForumPostDto>();
    }


    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("watch_topic_text")]
    public string WatchTopicText { get; set; }


    [JsonProperty("is_customer_allowed_to_edit_topic")]
    public bool IsCustomerAllowedToEditTopic { get; set; }

    [JsonProperty("is_customer_allowed_to_delete_topic")]
    public bool IsCustomerAllowedToDeleteTopic { get; set; }

    [JsonProperty("is_customer_allowed_to_move_topic")]
    public bool IsCustomerAllowedToMoveTopic { get; set; }

    [JsonProperty("is_customer_allowed_to_subscribe")]
    public bool IsCustomerAllowedToSubscribe { get; set; }


    [JsonProperty("forum_posts")]
    public IList<ForumPostDto> ForumPosts { get; set; }

    [JsonProperty("posts_page_index")]
    public int PostsPageIndex { get; set; }

    [JsonProperty("posts_page_size")]
    public int PostsPageSize { get; set; }

    [JsonProperty("posts_total_records")]
    public int PostsTotalRecords { get; set; }
}
