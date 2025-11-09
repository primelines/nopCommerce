using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "OrderTotals")]
public partial record OrderTotalsDto : BaseNopDto
{
    public OrderTotalsDto()
    {
        TaxRates = new List<TaxRateDto>();
        GiftCards = new List<GiftCard>();
    }

    [JsonProperty("is_editable")]
    public bool IsEditable { get; set; }


    [JsonProperty("sub_total")]
    public string SubTotal { get; set; }


    [JsonProperty("sub_total_discount")]
    public string SubTotalDiscount { get; set; }


    [JsonProperty("shipping")]
    public string Shipping { get; set; }

    [JsonProperty("requires_shipping")]
    public bool RequiresShipping { get; set; }

    [JsonProperty("selected_shipping_method")]
    public string SelectedShippingMethod { get; set; }

    [JsonProperty("hide_shipping_total")]
    public bool HideShippingTotal { get; set; }


    [JsonProperty("payment_method_additional_fee")]
    public string PaymentMethodAdditionalFee { get; set; }


    [JsonProperty("tax")]
    public string Tax { get; set; }

    [JsonProperty("tax_rates")]
    public IList<TaxRateDto> TaxRates { get; set; }

    [JsonProperty("display_tax")]
    public bool DisplayTax { get; set; }

    [JsonProperty("display_tax_rates")]
    public bool DisplayTaxRates { get; set; }


    [JsonProperty("gift_cards")]
    public IList<GiftCard> GiftCards { get; set; }


    [JsonProperty("order_total_discount")]
    public string OrderTotalDiscount { get; set; }

    [JsonProperty("redeemed_reward_points")]
    public int RedeemedRewardPoints { get; set; }

    [JsonProperty("redeemed_reward_points_amount")]
    public string RedeemedRewardPointsAmount { get; set; }


    [JsonProperty("will_earn_reward_points")]
    public int WillEarnRewardPoints { get; set; }


    [JsonProperty("order_total")]
    public string OrderTotal { get; set; }

#region Nested classes

    public partial record GiftCard : BaseNopEntityDto
    {

    [JsonProperty("coupon_code")]
        public string CouponCode { get; set; }

    [JsonProperty("amount")]
        public string Amount { get; set; }

    [JsonProperty("remaining")]
        public string Remaining { get; set; }
    }

    #endregion
}
