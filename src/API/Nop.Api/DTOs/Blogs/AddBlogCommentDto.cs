using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "AddBlogComment")]
public partial record AddBlogCommentDto : BaseNopEntityDto
{

    [JsonProperty("comment_text")]
    public string CommentText { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
