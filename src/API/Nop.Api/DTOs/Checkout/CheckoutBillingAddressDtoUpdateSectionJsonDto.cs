using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutBillingAddressUpdateSectionJson")]
public partial record CheckoutBillingAddressDtoUpdateSectionJsonDto : BaseNopDto
{
    [JsonProperty("name")]
	public string? Name { get; set; }

    [JsonProperty("view_name")]
	public string? ViewName { get; set; }

    [JsonProperty("model")]
	public CheckoutBillingAddressDto Model { get; set; }
}
