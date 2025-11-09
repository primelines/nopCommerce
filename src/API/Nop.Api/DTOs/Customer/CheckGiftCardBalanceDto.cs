using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CheckGiftCardBalance")]
public partial record CheckGiftCardBalanceDto : BaseNopDto
{

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }

    [NopResourceDisplayName("ShoppingCart.GiftCardCouponCode.Tooltip")]

    [JsonProperty("gift_card_code")]
    public string GiftCardCode { get; set; }
}
