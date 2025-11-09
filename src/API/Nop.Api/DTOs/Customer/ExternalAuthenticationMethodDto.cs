using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "ExternalAuthenticationMethod")]
public partial record ExternalAuthenticationMethodDto : BaseNopDto
{

    [JsonProperty("view_component")]
    public Type ViewComponent { get; set; }
}
