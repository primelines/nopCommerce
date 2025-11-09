using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutBillingAddressNextStepResponse")]
public partial record CheckoutBillingAddressDtoNextStepResponse : BaseNopDto
{
    [JsonProperty("update_section_dto")]
    public CheckoutBillingAddressDtoUpdateSectionJsonDto UpdateSectionDto { get; set; }

    [JsonProperty("wrong_billing_address")]
    public bool WrongBillingAddress { get; set; }

    [JsonProperty("goto_section")]
    public string GotoSection { get; set; }
}
