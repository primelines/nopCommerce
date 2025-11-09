using Newtonsoft.Json;

namespace Nop.Api.Framework.Dtos;

[JsonObject(Title = "SelectListItem")]
public partial record SelectListItemDto
{

	[JsonProperty("disabled")]
	public bool Disabled { get; set; }

	[JsonProperty("group")]
	public SelectListGroupDto Group { get; set; }

	[JsonProperty("selected")]
	public bool Selected { get; set; }

	[JsonProperty("text")]
	public string? Text { get; set; }

	[JsonProperty("value")]
	public string? Value { get; set; }


}
