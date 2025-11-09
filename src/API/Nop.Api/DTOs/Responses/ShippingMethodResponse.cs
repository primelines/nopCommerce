using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ShippingMethodResponse")]
public partial record ShippingMethodResponse : BaseNopEntityDto
{

    [JsonProperty("dto")]
    public CheckoutShippingMethodDto Dto { get; set; }

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }


}
