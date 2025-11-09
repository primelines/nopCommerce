using Newtonsoft.Json;
using Nop.Api.DTOs.Customer;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "AddressEditResponse")]
public partial record AddressEditResponse : BaseNopDto
{
    [JsonProperty("model")]
    public CustomerAddressEditDto Model { get; set; }

    [JsonProperty("errors")]
    public string Errors { get; set; }
}
