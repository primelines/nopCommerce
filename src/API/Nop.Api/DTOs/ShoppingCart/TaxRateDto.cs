using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.ShoppingCart;


[JsonObject(Title = "TaxRate")]
public partial record TaxRateDto : BaseNopDto
{

    [JsonProperty("rate")]
    public string Rate { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}


