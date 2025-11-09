using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerAddressEdit")]
public partial record CustomerAddressEditDto : BaseNopDto
{
    public CustomerAddressEditDto()
    {
        Address = new AddressDto();
    }


    [JsonProperty("address")]
    public AddressDto Address { get; set; }
}
