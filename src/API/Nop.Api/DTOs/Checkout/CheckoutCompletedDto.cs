using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutCompleted")]
public partial record CheckoutCompletedDto : BaseNopDto
{

    [JsonProperty("order_id")]
    public int OrderId { get; set; }

    [JsonProperty("custom_order_number")]
    public string CustomOrderNumber { get; set; }

    [JsonProperty("one_page_checkout_enabled")]
    public bool OnePageCheckoutEnabled { get; set; }
}
