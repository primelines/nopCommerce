using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdBrand")]
public record JsonLdBrandDto : JsonLdDto
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "Brand";

    [JsonProperty("name")]
    public string Name { get; set; }

    #endregion
}
