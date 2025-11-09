using Newtonsoft.Json;
using Nop.Api.DTOs.Customer;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "InfoResponse")]
public partial record InfoResponse : BaseNopDto
{
    [JsonProperty("dto")]
    public CustomerInfoDto Dto { get; set; }

    [JsonProperty("errors")]
    public string Errors { get; set; }

}
