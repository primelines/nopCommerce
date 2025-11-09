using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumRow")]
public partial record ForumRowDto : BaseNopDto
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("num_topics")]
    public int NumTopics { get; set; }

    [JsonProperty("num_posts")]
    public int NumPosts { get; set; }

    [JsonProperty("last_post_id")]
    public int LastPostId { get; set; }
}
