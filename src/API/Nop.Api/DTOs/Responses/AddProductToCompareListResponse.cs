using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "AddProductToCompareListResponse")]
public partial record AddProductToCompareListResponse : BaseNopDto
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
