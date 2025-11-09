using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.ShoppingCart;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "SelectShippingOptionResponse")]
public partial record SelectShippingOptionResponse : BaseNopDto
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("dto")]
    public OrderTotalsDto Dto { get; set; }

    [JsonProperty("errors")]
    public string Errors { get; set; }
}
