using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "PaymentInfoResponse")]
public partial record PaymentInfoResponse : BaseNopDto
{

    [JsonProperty("checkout_confirm")]
    public CheckoutConfirmDto CheckoutConfirm { get; set; }

    [JsonProperty("checkout_payment_info")]
    public CheckoutPaymentInfoDto CheckoutPaymentInfo { get; set; }


}
