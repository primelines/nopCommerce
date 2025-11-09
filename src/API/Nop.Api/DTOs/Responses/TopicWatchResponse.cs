using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "TopicWatchResponse")]
public partial record TopicWatchResponse : BaseNopDto
{
    [JsonProperty("subscribed")]
    public bool Subscribed { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("error")]
    public bool Error { get; set; }
}
