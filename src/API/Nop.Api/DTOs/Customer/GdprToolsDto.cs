using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "GdprTools")]
public partial record GdprToolsDto : BaseNopDto
{

    [JsonProperty("result")]
    public string Result { get; set; }
}
