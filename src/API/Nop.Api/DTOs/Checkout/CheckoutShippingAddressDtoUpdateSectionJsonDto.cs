using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutShippingAddressUpdateSectionJson")]
public partial record CheckoutShippingAddressDtoUpdateSectionJsonDto : BaseNopDto
{
    [JsonProperty("name")]
	public string? Name { get; set; }

    [JsonProperty("view_name")]
	public string? ViewName { get; set; }

    [JsonProperty("model")]
	public CheckoutShippingAddressDto Model { get; set; }
}
