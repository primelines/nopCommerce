using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdPerson")]
public record JsonLdPersonDto : JsonLdDto
{
    #region Properties


    [JsonProperty("@type")]
    public static string Type => "Person";


    [JsonProperty("name")]
    public string Name { get; set; }

    #endregion
}
