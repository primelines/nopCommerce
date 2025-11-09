using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutShippingAddressNextStepResponse")]
public partial record CheckoutShippingAddressDtoNextStepResponse : BaseNopDto
{

    [JsonProperty("update_section")]
    public CheckoutShippingAddressDtoUpdateSectionJsonDto UpdateSection { get; set; }

    [JsonProperty("wrong_billing_address")]
    public bool WrongBillingAddress { get; set; }

    [JsonProperty("goto_section")]
    public string GotoSection { get; set; }

}
