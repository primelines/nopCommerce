using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdBreadcrumbList")]
public record JsonLdBreadcrumbListDto : JsonLdDto
{
    #region Ctor

    public JsonLdBreadcrumbListDto()
    {
        ItemListElement = new List<JsonLdBreadcrumbListItemDto>();
    }

    #endregion

    #region Properties

    [JsonProperty("@context")]
    public static string Context => "https://schema.org";

    [JsonProperty("@type")]
    public static string Type => "BreadcrumbList";

    [JsonProperty("item_list_element")]
    public IList<JsonLdBreadcrumbListItemDto> ItemListElement { get; set; }

    #endregion
}
