using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
namespace Nop.Api.DTOs.Catalog;
[JsonObject(Title = "TierPrice")]
public partial record TierPriceDto : BaseNopDto
{

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("price_value")]
    public decimal PriceValue { get; set; }


    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
