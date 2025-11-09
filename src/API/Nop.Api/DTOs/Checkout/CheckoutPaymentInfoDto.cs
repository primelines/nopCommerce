using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutPaymentInfo")]
public partial record CheckoutPaymentInfoDto : BaseNopDto
{

    [JsonProperty("payment_view_component")]
    public Type PaymentViewComponent { get; set; }

    /// <summary>
    /// Used on one-page checkout page
    /// </summary>

    [JsonProperty("display_order_totals")]
    public bool DisplayOrderTotals { get; set; }
}
