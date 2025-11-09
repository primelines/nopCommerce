using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogPostTag")]
public partial record BlogPostTagDto : BaseNopDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("blog_post_count")]
    public int BlogPostCount { get; set; }
}
