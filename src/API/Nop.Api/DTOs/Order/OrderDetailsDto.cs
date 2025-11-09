using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;
using Nop.Api.DTOs.ShoppingCart;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "OrderDetails")]
public partial record OrderDetailsDto : BaseNopEntityDto
{
    public OrderDetailsDto()
    {
        TaxRates = new List<TaxRateDto>();
        GiftCards = new List<GiftCardOverViewDto>();
        Items = new List<OrderItemDto>();
        OrderNotes = new List<OrderNoteDto>();
        Shipments = new List<ShipmentBriefDto>();

        BillingAddress = new AddressDto();
        ShippingAddress = new AddressDto();
        PickupAddress = new AddressDto();

        CustomValues = new Dictionary<string, object>();
    }


    [JsonProperty("print_mode")]
    public bool PrintMode { get; set; }

    [JsonProperty("pdf_invoice_disabled")]
    public bool PdfInvoiceDisabled { get; set; }


    [JsonProperty("custom_order_number")]
    public string CustomOrderNumber { get; set; }


    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }


    [JsonProperty("order_status")]
    public string OrderStatus { get; set; }


    [JsonProperty("is_re_order_allowed")]
    public bool IsReOrderAllowed { get; set; }


    [JsonProperty("is_return_request_allowed")]
    public bool IsReturnRequestAllowed { get; set; }


    [JsonProperty("is_shippable")]
    public bool IsShippable { get; set; }

    [JsonProperty("pickup_in_store")]
    public bool PickupInStore { get; set; }

    [JsonProperty("pickup_address")]
    public AddressDto PickupAddress { get; set; }

    [JsonProperty("shipping_status")]
    public string ShippingStatus { get; set; }

    [JsonProperty("shipping_address")]
    public AddressDto ShippingAddress { get; set; }

    [JsonProperty("shipping_method")]
    public string ShippingMethod { get; set; }

    [JsonProperty("shipments")]
    public IList<ShipmentBriefDto> Shipments { get; set; }


    [JsonProperty("billing_address")]
    public AddressDto BillingAddress { get; set; }


    [JsonProperty("vat_number")]
    public string VatNumber { get; set; }


    [JsonProperty("payment_method")]
    public string PaymentMethod { get; set; }

    [JsonProperty("payment_method_status")]
    public string PaymentMethodStatus { get; set; }

    [JsonProperty("can_re_post_process_payment")]
    public bool CanRePostProcessPayment { get; set; }
    public Dictionary<string, object> CustomValues { get; set; }


    [JsonProperty("order_subtotal")]
    public string OrderSubtotal { get; set; }

    [JsonProperty("order_subtotal_value")]
    public decimal OrderSubtotalValue { get; set; }

    [JsonProperty("order_sub_total_discount")]
    public string OrderSubTotalDiscount { get; set; }

    [JsonProperty("order_sub_total_discount_value")]
    public decimal OrderSubTotalDiscountValue { get; set; }

    [JsonProperty("order_shipping")]
    public string OrderShipping { get; set; }

    [JsonProperty("order_shipping_value")]
    public decimal OrderShippingValue { get; set; }

    [JsonProperty("payment_method_additional_fee")]
    public string PaymentMethodAdditionalFee { get; set; }

    [JsonProperty("payment_method_additional_fee_value")]
    public decimal PaymentMethodAdditionalFeeValue { get; set; }

    [JsonProperty("checkout_attribute_info")]
    public string CheckoutAttributeInfo { get; set; }


    [JsonProperty("prices_include_tax")]
    public bool PricesIncludeTax { get; set; }

    [JsonProperty("display_tax_shipping_info")]
    public bool DisplayTaxShippingInfo { get; set; }

    [JsonProperty("tax")]
    public string Tax { get; set; }

    [JsonProperty("tax_rates")]
    public IList<TaxRateDto> TaxRates { get; set; }

    [JsonProperty("display_tax")]
    public bool DisplayTax { get; set; }

    [JsonProperty("display_tax_rates")]
    public bool DisplayTaxRates { get; set; }


    [JsonProperty("order_total_discount")]
    public string OrderTotalDiscount { get; set; }

    [JsonProperty("order_total_discount_value")]
    public decimal OrderTotalDiscountValue { get; set; }

    [JsonProperty("redeemed_reward_points")]
    public int RedeemedRewardPoints { get; set; }

    [JsonProperty("redeemed_reward_points_amount")]
    public string RedeemedRewardPointsAmount { get; set; }

    [JsonProperty("order_total")]
    public string OrderTotal { get; set; }

    [JsonProperty("order_total_value")]
    public decimal OrderTotalValue { get; set; }


    [JsonProperty("gift_cards")]
    public IList<GiftCardOverViewDto> GiftCards { get; set; }


    [JsonProperty("show_sku")]
    public bool ShowSku { get; set; }

    [JsonProperty("items")]
    public IList<OrderItemDto> Items { get; set; }


    [JsonProperty("order_notes")]
    public IList<OrderNoteDto> OrderNotes { get; set; }


    [JsonProperty("show_vendor_name")]
    public bool ShowVendorName { get; set; }

    [JsonProperty("show_product_thumbnail")]
    public bool ShowProductThumbnail { get; set; }

}
