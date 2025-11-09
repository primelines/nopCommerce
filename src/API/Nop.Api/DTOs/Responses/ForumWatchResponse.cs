using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ForumWatchResponse")]
public partial record ForumWatchResponse : BaseNopDto
{
    [JsonProperty("subscribed")]
    public bool Subscribed { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("error")]
    public bool Error { get; set; }
}
