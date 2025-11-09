using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Logo")]
public partial record LogoDto : BaseNopDto
{

    [JsonProperty("store_name")]
    public string StoreName { get; set; }


    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }
}
