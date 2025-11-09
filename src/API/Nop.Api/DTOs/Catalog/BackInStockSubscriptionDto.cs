using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "BackInStockSubscription")]
public partial record BackInStockSubscriptionDto : BaseNopEntityDto
{

    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }
}
