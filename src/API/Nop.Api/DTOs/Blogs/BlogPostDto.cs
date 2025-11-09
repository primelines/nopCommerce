using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogPost")]
public partial record BlogPostDto : BaseNopEntityDto
{
    public BlogPostDto()
    {
        Tags = new List<string>();
        Comments = new List<BlogCommentDto>();
        AddNewComment = new AddBlogCommentDto();
    }


    [JsonProperty("meta_keywords")]
    public string MetaKeywords { get; set; }

    [JsonProperty("meta_description")]
    public string MetaDescription { get; set; }

    [JsonProperty("meta_title")]
    public string MetaTitle { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("body_overview")]
    public string BodyOverview { get; set; }

    [JsonProperty("allow_comments")]
    public bool AllowComments { get; set; }

    [JsonProperty("prevent_not_registered_users_to_leave_comments")]
    public bool PreventNotRegisteredUsersToLeaveComments { get; set; }

    [JsonProperty("number_of_comments")]
    public int NumberOfComments { get; set; }

    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }

    [JsonProperty("tags")]
    public IList<string> Tags { get; set; }

    [JsonProperty("comments")]
    public IList<BlogCommentDto> Comments { get; set; }

    [JsonProperty("add_new_comment")]
    public AddBlogCommentDto AddNewComment { get; set; }
}
