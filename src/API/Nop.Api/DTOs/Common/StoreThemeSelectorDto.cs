using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "StoreThemeSelector")]
public partial record StoreThemeSelectorDto : BaseNopDto
{
    public StoreThemeSelectorDto()
    {
        AvailableStoreThemes = new List<StoreThemeDto>();
    }


    [JsonProperty("available_store_themes")]
    public IList<StoreThemeDto> AvailableStoreThemes { get; set; }


    [JsonProperty("current_store_theme")]
    public StoreThemeDto CurrentStoreTheme { get; set; }
}
