using Newtonsoft.Json;
using Nop.Core.Domain.Customers;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "HeaderLinks")]
public partial record HeaderLinksDto : BaseNopDto
{

    [JsonProperty("is_authenticated")]
    public bool IsAuthenticated { get; set; }

    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }


    [JsonProperty("shopping_cart_enabled")]
    public bool ShoppingCartEnabled { get; set; }

    [JsonProperty("shopping_cart_items")]
    public int ShoppingCartItems { get; set; }


    [JsonProperty("wishlist_enabled")]
    public bool WishlistEnabled { get; set; }

    [JsonProperty("wishlist_items")]
    public int WishlistItems { get; set; }


    [JsonProperty("allow_private_messages")]
    public bool AllowPrivateMessages { get; set; }

    [JsonProperty("unread_private_messages")]
    public string UnreadPrivateMessages { get; set; }

    [JsonProperty("alert_message")]
    public string AlertMessage { get; set; }

    [JsonProperty("registration_type")]
    public UserRegistrationType RegistrationType { get; set; }
}
