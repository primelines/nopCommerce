using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Orders;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "AddToCart")]
public partial record AddToCartDto : BaseNopDto
{
    public AddToCartDto()
    {
        AllowedQuantities = new List<SelectListItemDto>();
    }

    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    //qty

    [JsonProperty("entered_quantity")]
    public int EnteredQuantity { get; set; }

    [JsonProperty("minimum_quantity_notification")]
    public string MinimumQuantityNotification { get; set; }

    [JsonProperty("allowed_quantities")]
    public List<SelectListItemDto> AllowedQuantities { get; set; }

    //price entered by customers

    [JsonProperty("customer_enters_price")]
    public bool CustomerEntersPrice { get; set; }

    [JsonProperty("customer_entered_price")]
    public decimal CustomerEnteredPrice { get; set; }

    [JsonProperty("customer_entered_price_range")]
    public string CustomerEnteredPriceRange { get; set; }


    [JsonProperty("disable_buy_button")]
    public bool DisableBuyButton { get; set; }

    [JsonProperty("disable_wishlist_button")]
    public bool DisableWishlistButton { get; set; }

    //pre-order

    [JsonProperty("available_for_pre_order")]
    public bool AvailableForPreOrder { get; set; }

    [JsonProperty("pre_order_availability_start_date_time_utc")]
    public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

    [JsonProperty("pre_order_availability_start_date_time_user_time")]
    public string PreOrderAvailabilityStartDateTimeUserTime { get; set; }

    //updating existing shopping cart or wishlist item?

    [JsonProperty("updated_shopping_cart_item_id")]
    public int UpdatedShoppingCartItemId { get; set; }

    [JsonProperty("update_shopping_cart_item_type")]
    public ShoppingCartType? UpdateShoppingCartItemType { get; set; }
}

