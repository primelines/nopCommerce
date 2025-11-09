using Newtonsoft.Json;
using Nop.Api.DTOs.ShoppingCart;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductEstimateShipping")]
public partial record ProductEstimateShippingDto : EstimateShippingDto
{

    [JsonProperty("product_id")]
    public int ProductId { get; set; }
}
