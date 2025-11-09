using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ShippingAddressResponse")]
public partial record ShippingAddressResponse : BaseNopEntityDto
{


    [JsonProperty("model")]
    public CheckoutShippingAddressDto Dto { get; set; }

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }

}
