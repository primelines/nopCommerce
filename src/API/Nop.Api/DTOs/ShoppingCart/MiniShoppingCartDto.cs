using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "MiniShoppingCart")]
public partial record MiniShoppingCartDto : BaseNopDto
{
    public MiniShoppingCartDto()
    {
        Items = new List<ShoppingCartItemOverviewDto>();
    }


    [JsonProperty("items")]
    public IList<ShoppingCartItemOverviewDto> Items { get; set; }

    [JsonProperty("total_products")]
    public int TotalProducts { get; set; }

    [JsonProperty("sub_total")]
    public string SubTotal { get; set; }

    [JsonProperty("sub_total_value")]
    public decimal SubTotalValue { get; set; }

    [JsonProperty("display_shopping_cart_button")]
    public bool DisplayShoppingCartButton { get; set; }

    [JsonProperty("display_checkout_button")]
    public bool DisplayCheckoutButton { get; set; }

    [JsonProperty("current_customer_is_guest")]
    public bool CurrentCustomerIsGuest { get; set; }

    [JsonProperty("anonymous_checkout_allowed")]
    public bool AnonymousCheckoutAllowed { get; set; }

    [JsonProperty("show_product_images")]
    public bool ShowProductImages { get; set; }

}
