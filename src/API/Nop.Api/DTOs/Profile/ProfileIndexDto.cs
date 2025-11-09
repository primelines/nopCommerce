using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Profile;

[JsonObject(Title = "ProfileIndex")]
public partial record ProfileIndexDto : BaseNopDto
{

    [JsonProperty("customer_profile_id")]
    public int CustomerProfileId { get; set; }

    [JsonProperty("profile_title")]
    public string ProfileTitle { get; set; }

    [JsonProperty("posts_page")]
    public int PostsPage { get; set; }

    [JsonProperty("paging_posts")]
    public bool PagingPosts { get; set; }

    [JsonProperty("forums_enabled")]
    public bool ForumsEnabled { get; set; }
}
