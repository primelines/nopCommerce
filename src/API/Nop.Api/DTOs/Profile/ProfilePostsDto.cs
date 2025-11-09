using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Profile;

[JsonObject(Title = "ProfilePosts")]
public partial record ProfilePostsDto : BaseNopDto
{

    [JsonProperty("posts")]
    public IList<PostsDto> Posts { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }
}
