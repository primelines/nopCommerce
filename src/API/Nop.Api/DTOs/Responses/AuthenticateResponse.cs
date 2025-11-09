using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "AuthenticateResponse")]
public partial record AuthenticateResponse : BaseNopDto
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }
}
