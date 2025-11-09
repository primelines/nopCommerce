using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutConfirm")]
public partial record CheckoutConfirmDto : BaseNopDto
{
    public CheckoutConfirmDto()
    {
        Warnings = new List<string>();
    }


    [JsonProperty("terms_of_service_on_order_confirm_page")]
    public bool TermsOfServiceOnOrderConfirmPage { get; set; }

    [JsonProperty("terms_of_service_popup")]
    public bool TermsOfServicePopup { get; set; }

    [JsonProperty("min_order_total_warning")]
    public string MinOrderTotalWarning { get; set; }

    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }
}
