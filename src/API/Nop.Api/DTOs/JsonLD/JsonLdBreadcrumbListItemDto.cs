using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdBreadcrumbListItem")]
public record JsonLdBreadcrumbListItemDto : JsonLdDto
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "ListItem";


    [JsonProperty("position")]
    public int Position { get; set; }


    [JsonProperty("item")]
    public JsonLdBreadcrumbItemDto Item { get; set; }

    #endregion
}
