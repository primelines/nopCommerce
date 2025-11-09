using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "NewsComment")]
public partial record NewsCommentDto : BaseNopEntityDto
{

    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }


    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }


    [JsonProperty("customer_avatar_url")]
    public string CustomerAvatarUrl { get; set; }


    [JsonProperty("comment_title")]
    public string CommentTitle { get; set; }


    [JsonProperty("comment_text")]
    public string CommentText { get; set; }


    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }


    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }
}
