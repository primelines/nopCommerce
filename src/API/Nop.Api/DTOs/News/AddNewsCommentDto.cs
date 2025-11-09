using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "AddNewsComment")]
public partial record AddNewsCommentDto : BaseNopDto
{

    [JsonProperty("comment_title")]
    public string CommentTitle { get; set; }


    [JsonProperty("comment_text")]
    public string CommentText { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
