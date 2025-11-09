using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutConfirmNextStepResponse")]
public partial record CheckoutConfirmDtoNextStepResponse : BaseNopDto
{
    [JsonProperty("update_section")]
    public CheckoutConfirmDtoUpdateSectionJsonDto UpdateSection { get; set; }

    [JsonProperty("wrong_billing_address")]
    public bool WrongBillingAddress { get; set; }

    [JsonProperty("goto_section")]
    public string GotoSection { get; set; }
}
