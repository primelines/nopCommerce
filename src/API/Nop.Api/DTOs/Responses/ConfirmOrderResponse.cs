using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ConfirmOrderResponse")]
public partial record ConfirmOrderResponse : BaseNopEntityDto
{
    [JsonProperty("dto")]
    public CheckoutConfirmDto Dto { get; set; }

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }
}
