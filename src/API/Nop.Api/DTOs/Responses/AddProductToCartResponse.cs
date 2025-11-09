using Newtonsoft.Json;
using Nop.Api.DTOs.ShoppingCart;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "AddProductToCartResponse")]
public partial record AddProductToCartResponse : BaseNopDto
{
    [JsonProperty("errors")]
    public string Errors { get; set; }

    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("dto")]
    public MiniShoppingCartDto Dto { get; set; }
}
