using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerAddressList")]
public partial record CustomerAddressListDto : BaseNopDto
{
    public CustomerAddressListDto()
    {
        Addresses = new List<AddressDto>();
    }


    [JsonProperty("addresses")]
    public IList<AddressDto> Addresses { get; set; }
}
