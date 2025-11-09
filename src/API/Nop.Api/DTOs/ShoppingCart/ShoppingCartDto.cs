using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "ShoppingCart")]
public partial record ShoppingCartDto : BaseNopDto
{
    public ShoppingCartDto()
    {
        Items = new List<ShoppingCartItemDto>();
        Warnings = new List<string>();
        DiscountBox = new DiscountBoxModel();
        GiftCardBox = new GiftCardBoxModel();
        CheckoutAttributes = new List<CheckoutAttributeDto>();
        OrderReviewData = new OrderReviewDataModel();

        ButtonPaymentMethodViewComponents = new List<Type>();
    }


    [JsonProperty("one_page_checkout_enabled")]
    public bool OnePageCheckoutEnabled { get; set; }


    [JsonProperty("show_sku")]
    public bool ShowSku { get; set; }

    [JsonProperty("show_product_images")]
    public bool ShowProductImages { get; set; }

    [JsonProperty("is_editable")]
    public bool IsEditable { get; set; }

    [JsonProperty("items")]
    public IList<ShoppingCartItemDto> Items { get; set; }


    [JsonProperty("checkout_attributes")]
    public IList<CheckoutAttributeDto> CheckoutAttributes { get; set; }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }

    [JsonProperty("min_order_subtotal_warning")]
    public string MinOrderSubtotalWarning { get; set; }

    [JsonProperty("display_tax_shipping_info")]
    public bool DisplayTaxShippingInfo { get; set; }

    [JsonProperty("terms_of_service_on_shopping_cart_page")]
    public bool TermsOfServiceOnShoppingCartPage { get; set; }

    [JsonProperty("terms_of_service_on_order_confirm_page")]
    public bool TermsOfServiceOnOrderConfirmPage { get; set; }

    [JsonProperty("terms_of_service_popup")]
    public bool TermsOfServicePopup { get; set; }

    [JsonProperty("discount_box")]
    public DiscountBoxModel DiscountBox { get; set; }

    [JsonProperty("gift_card_box")]
    public GiftCardBoxModel GiftCardBox { get; set; }

    [JsonProperty("order_review_data")]
    public OrderReviewDataModel OrderReviewData { get; set; }


    [JsonProperty("button_payment_method_view_components")]
    public IList<Type> ButtonPaymentMethodViewComponents { get; set; }


    [JsonProperty("hide_checkout_button")]
    public bool HideCheckoutButton { get; set; }

    [JsonProperty("show_vendor_name")]
    public bool ShowVendorName { get; set; }

#region Nested Classes

    public partial record CheckoutAttributeDto : BaseNopEntityDto
    {
        public CheckoutAttributeDto()
        {
            AllowedFileExtensions = new List<string>();
            Values = new List<CheckoutAttributeValueDto>();
        }


    [JsonProperty("name")]
        public string Name { get; set; }


    [JsonProperty("default_value")]
        public string DefaultValue { get; set; }


    [JsonProperty("text_prompt")]
        public string TextPrompt { get; set; }


    [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Selected day value for datepicker
        /// </summary>

    [JsonProperty("selected_day")]
        public int? SelectedDay { get; set; }
        /// <summary>
        /// Selected month value for datepicker
        /// </summary>

    [JsonProperty("selected_month")]
        public int? SelectedMonth { get; set; }
        /// <summary>
        /// Selected year value for datepicker
        /// </summary>

    [JsonProperty("selected_year")]
        public int? SelectedYear { get; set; }

        /// <summary>
        /// Allowed file extensions for customer uploaded files
        /// </summary>

    [JsonProperty("allowed_file_extensions")]
        public IList<string> AllowedFileExtensions { get; set; }


    [JsonProperty("attribute_control_type")]
        public AttributeControlType AttributeControlType { get; set; }


    [JsonProperty("values")]
        public IList<CheckoutAttributeValueDto> Values { get; set; }
    }

    public partial record CheckoutAttributeValueDto : BaseNopEntityDto
    {

    [JsonProperty("name")]
        public string Name { get; set; }


    [JsonProperty("color_squares_rgb")]
        public string ColorSquaresRgb { get; set; }


    [JsonProperty("price_adjustment")]
        public string PriceAdjustment { get; set; }


    [JsonProperty("is_pre_selected")]
        public bool IsPreSelected { get; set; }
    }

    public partial record DiscountBoxModel : BaseNopDto
    {
        public DiscountBoxModel()
        {
            AppliedDiscountsWithCodes = new List<DiscountInfoModel>();
            Messages = new List<string>();
        }


    [JsonProperty("applied_discounts_with_codes")]
        public List<DiscountInfoModel> AppliedDiscountsWithCodes { get; set; }

    [JsonProperty("display")]
        public bool Display { get; set; }

    [JsonProperty("messages")]
        public List<string> Messages { get; set; }

    [JsonProperty("is_applied")]
        public bool IsApplied { get; set; }

        public partial record DiscountInfoModel : BaseNopEntityDto
        {

    [JsonProperty("coupon_code")]
            public string CouponCode { get; set; }
        }
    }

    public partial record GiftCardBoxModel : BaseNopDto
    {

    [JsonProperty("display")]
        public bool Display { get; set; }

    [JsonProperty("message")]
        public string Message { get; set; }

    [JsonProperty("is_applied")]
        public bool IsApplied { get; set; }
    }

    public partial record OrderReviewDataModel : BaseNopDto
    {
        public OrderReviewDataModel()
        {
            BillingAddress = new AddressDto();
            ShippingAddress = new AddressDto();
            PickupAddress = new AddressDto();
            CustomValues = new Dictionary<string, object>();
        }

    [JsonProperty("display")]
        public bool Display { get; set; }


    [JsonProperty("billing_address")]
        public AddressDto BillingAddress { get; set; }


    [JsonProperty("is_shippable")]
        public bool IsShippable { get; set; }

    [JsonProperty("shipping_address")]
        public AddressDto ShippingAddress { get; set; }

    [JsonProperty("selected_pickup_in_store")]
        public bool SelectedPickupInStore { get; set; }

    [JsonProperty("pickup_address")]
        public AddressDto PickupAddress { get; set; }

    [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; }


    [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        public Dictionary<string, object> CustomValues { get; set; }
    }

    #endregion
}
