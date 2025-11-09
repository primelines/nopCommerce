using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdBreadcrumbItem")]
public record JsonLdBreadcrumbItemDto : JsonLdDto
{
    #region Properties

    [JsonProperty("@id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    #endregion
}
