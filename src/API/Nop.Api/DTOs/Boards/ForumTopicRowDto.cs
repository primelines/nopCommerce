using Newtonsoft.Json;
using Nop.Core.Domain.Forums;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumTopicRow")]
public partial record ForumTopicRowDto : BaseNopDto
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("last_post_id")]
    public int LastPostId { get; set; }


    [JsonProperty("num_posts")]
    public int NumPosts { get; set; }

    [JsonProperty("views")]
    public int Views { get; set; }

    [JsonProperty("votes")]
    public int Votes { get; set; }

    [JsonProperty("num_replies")]
    public int NumReplies { get; set; }

    [JsonProperty("forum_topic_type")]
    public ForumTopicType ForumTopicType { get; set; }


    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }

    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }

    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }

    //posts

    [JsonProperty("total_post_pages")]
    public int TotalPostPages { get; set; }
}
