using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CompletedResponse")]
public partial record CompletedResponse : BaseNopEntityDto
{

    [JsonProperty("dto")]
    public CheckoutCompletedDto Dto { get; set; }

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }
}
