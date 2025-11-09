using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutConfirmUpdateSectionJson")]
public partial record CheckoutConfirmDtoUpdateSectionJsonDto : BaseNopDto
{
    [JsonProperty("name")]
	public string? Name { get; set; }

    [JsonProperty("view_name")]
	public string? ViewName { get; set; }

    [JsonProperty("model")]
	public CheckoutConfirmDto Model { get; set; }
}
