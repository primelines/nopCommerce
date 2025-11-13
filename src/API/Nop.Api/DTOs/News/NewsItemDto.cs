using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "NewsItem")]
public partial record NewsItemDto : BaseNopEntityDto
{
    public NewsItemDto()
    {
        Comments = new List<NewsCommentDto>();
        AddNewComment = new AddNewsCommentDto();
    }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("short")]
    public string Short { get; set; }

    [JsonProperty("full")]
    public string Full { get; set; }

    [JsonProperty("allow_comments")]
    public bool AllowComments { get; set; }

    [JsonProperty("prevent_not_registered_users_to_leave_comments")]
    public bool PreventNotRegisteredUsersToLeaveComments { get; set; }

    [JsonProperty("number_of_comments")]
    public int NumberOfComments { get; set; }

    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }


    [JsonProperty("comments")]
    public IList<NewsCommentDto> Comments { get; set; }

    [JsonProperty("add_new_comment")]
    public AddNewsCommentDto AddNewComment { get; set; }
}
