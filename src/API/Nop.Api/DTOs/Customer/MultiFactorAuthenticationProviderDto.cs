using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "MultiFactorAuthenticationProvider")]
public partial record MultiFactorAuthenticationProviderDto : BaseNopEntityDto
{

    [JsonProperty("selected")]
    public bool Selected { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("system_name")]
    public string SystemName { get; set; }


    [JsonProperty("logo_url")]
    public string LogoUrl { get; set; }


    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("view_component")]
    public Type ViewComponent { get; set; }
}
