using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "NewBillingAddressResponse")]
public partial record NewBillingAddressResponse : BaseNopEntityDto
{
    [JsonProperty("dto")]
    public CheckoutBillingAddressDto Dto { get; set; }

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }


}
