using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Language")]
public partial record LanguageDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("flag_image_file_name")]
    public string FlagImageFileName { get; set; }
}
