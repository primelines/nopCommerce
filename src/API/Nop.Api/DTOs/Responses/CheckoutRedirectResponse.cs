using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutRedirectResponse")]
public partial record CheckoutRedirectResponse : BaseNopEntityDto
{

    [JsonProperty("redirect_to_method")]
    public string RedirectToMethod { get; set; }


}
