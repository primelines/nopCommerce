using Newtonsoft.Json;

namespace Nop.Api.Framework.Dtos;

[JsonObject(Title = "SelectListGroup")]
public partial record SelectListGroupDto 
{

	[JsonProperty("disabled")]
	public bool Disabled { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }


}
