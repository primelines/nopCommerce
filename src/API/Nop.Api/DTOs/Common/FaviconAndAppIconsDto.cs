using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "FaviconAndAppIcons")]
public partial record FaviconAndAppIconsDto : BaseNopDto
{

    [JsonProperty("head_code")]
    public string HeadCode { get; set; }
}
