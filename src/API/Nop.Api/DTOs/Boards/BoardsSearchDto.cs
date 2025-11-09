using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "BoardsSearch")]
public partial record BoardsSearchDto : BaseNopDto
{
    public BoardsSearchDto()
    {
        LimitList = new List<SelectListItemDto>();
        ForumList = new List<SelectListItemDto>();
        WithinList = new List<SelectListItemDto>();
        ForumTopics = new List<ForumTopicRowDto>();
    }


    [JsonProperty("show_advanced_search")]
    public bool ShowAdvancedSearch { get; set; }


    [JsonProperty("search_terms")]
    public string SearchTerms { get; set; }


    [JsonProperty("forum_id")]
    public int? ForumId { get; set; }


    [JsonProperty("within")]
    public int? Within { get; set; }


    [JsonProperty("limit_days")]
    public int? LimitDays { get; set; }


    [JsonProperty("forum_topics")]
    public IList<ForumTopicRowDto> ForumTopics { get; set; }

    [JsonProperty("topic_page_size")]
    public int TopicPageSize { get; set; }

    [JsonProperty("topic_total_records")]
    public int TopicTotalRecords { get; set; }

    [JsonProperty("topic_page_index")]
    public int TopicPageIndex { get; set; }


    [JsonProperty("limit_list")]
    public List<SelectListItemDto> LimitList { get; set; }


    [JsonProperty("forum_list")]
    public List<SelectListItemDto> ForumList { get; set; }


    [JsonProperty("within_list")]
    public List<SelectListItemDto> WithinList { get; set; }


    [JsonProperty("forum_id_selected")]
    public int ForumIdSelected { get; set; }


    [JsonProperty("within_selected")]
    public int WithinSelected { get; set; }


    [JsonProperty("limit_days_selected")]
    public int LimitDaysSelected { get; set; }


    [JsonProperty("search_results_visible")]
    public bool SearchResultsVisible { get; set; }


    [JsonProperty("no_results_visisble")]
    public bool NoResultsVisisble { get; set; }


    [JsonProperty("error")]
    public string Error { get; set; }


    [JsonProperty("posts_page_size")]
    public int PostsPageSize { get; set; }


    [JsonProperty("allow_post_voting")]
    public bool AllowPostVoting { get; set; }
}
