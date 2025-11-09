using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "Wishlist")]
public partial record WishlistDto : BaseNopDto
{
    public WishlistDto()
    {
        Items = new List<ShoppingCartItemDto>();
        Warnings = new List<string>();
    }


    [JsonProperty("customer_guid")]
    public Guid CustomerGuid { get; set; }

    [JsonProperty("customer_fullname")]
    public string CustomerFullname { get; set; }


    [JsonProperty("email_wishlist_enabled")]
    public bool EmailWishlistEnabled { get; set; }


    [JsonProperty("show_sku")]
    public bool ShowSku { get; set; }


    [JsonProperty("show_product_images")]
    public bool ShowProductImages { get; set; }


    [JsonProperty("is_editable")]
    public bool IsEditable { get; set; }


    [JsonProperty("display_add_to_cart")]
    public bool DisplayAddToCart { get; set; }


    [JsonProperty("display_tax_shipping_info")]
    public bool DisplayTaxShippingInfo { get; set; }


    [JsonProperty("items")]
    public IList<ShoppingCartItemDto> Items { get; set; }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }

}
