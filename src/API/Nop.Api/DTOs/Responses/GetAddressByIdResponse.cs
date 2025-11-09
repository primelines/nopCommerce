using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "GetAddressByIdResponse")]
public partial record GetAddressByIdResponse : BaseNopDto
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("content_type")]
    public string ContentType { get; set; }
}
