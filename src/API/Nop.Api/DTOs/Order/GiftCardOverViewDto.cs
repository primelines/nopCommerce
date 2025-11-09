using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;
[JsonObject(Title = "GiftCardOverView")]
public partial record GiftCardOverViewDto : BaseNopDto
{

    [JsonProperty("coupon_code")]
    public string CouponCode { get; set; }

    [JsonProperty("amount")]
    public string Amount { get; set; }
}

