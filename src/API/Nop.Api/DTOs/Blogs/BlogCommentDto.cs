using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogComment")]
public partial record BlogCommentDto : BaseNopEntityDto
{

    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }


    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }


    [JsonProperty("customer_avatar_url")]
    public string CustomerAvatarUrl { get; set; }


    [JsonProperty("comment_text")]
    public string CommentText { get; set; }


    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }


    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }
}
