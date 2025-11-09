using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutProgress")]
public partial record CheckoutProgressDto : BaseNopDto
{

    [JsonProperty("checkout_progress_step")]
    public CheckoutProgressStep CheckoutProgressStep { get; set; }
}

public enum CheckoutProgressStep
{
    Cart,
    Address,
    Shipping,
    Payment,
    Confirm,
    Complete
}
