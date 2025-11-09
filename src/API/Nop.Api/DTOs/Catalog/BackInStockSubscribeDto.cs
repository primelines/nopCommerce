using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "BackInStockSubscribe")]
public partial record BackInStockSubscribeDto : BaseNopDto
{

    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
    public string ProductSeName { get; set; }


    [JsonProperty("is_current_customer_registered")]
    public bool IsCurrentCustomerRegistered { get; set; }

    [JsonProperty("subscription_allowed")]
    public bool SubscriptionAllowed { get; set; }

    [JsonProperty("already_subscribed")]
    public bool AlreadySubscribed { get; set; }


    [JsonProperty("maximum_back_in_stock_subscriptions")]
    public int MaximumBackInStockSubscriptions { get; set; }

    [JsonProperty("current_number_of_back_in_stock_subscriptions")]
    public int CurrentNumberOfBackInStockSubscriptions { get; set; }
}
