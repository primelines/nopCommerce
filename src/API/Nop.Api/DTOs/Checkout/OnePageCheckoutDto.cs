using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "OnePageCheckout")]
public partial record OnePageCheckoutDto : BaseNopDto
{

    [JsonProperty("shipping_required")]
    public bool ShippingRequired { get; set; }

    [JsonProperty("disable_billing_address_checkout_step")]
    public bool DisableBillingAddressCheckoutStep { get; set; }

    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }

    [JsonProperty("is_re_captcha_v3")]
    public bool IsReCaptchaV3 { get; set; }

    [JsonProperty("re_captcha_public_key")]
    public string ReCaptchaPublicKey { get; set; }


    [JsonProperty("billing_address")]
    public CheckoutBillingAddressDto BillingAddress { get; set; }
}
