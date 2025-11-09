using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckUsernameAvailabilityResponse")]
public partial record CheckUsernameAvailabilityResponse : BaseNopDto
{
    [JsonProperty("available")]
    public bool Available { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}
