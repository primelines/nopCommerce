using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "LanguageSelector")]
public partial record LanguageSelectorDto : BaseNopDto
{
    public LanguageSelectorDto()
    {
        AvailableLanguages = new List<LanguageDto>();
    }


    [JsonProperty("available_languages")]
    public IList<LanguageDto> AvailableLanguages { get; set; }


    [JsonProperty("current_language_id")]
    public int CurrentLanguageId { get; set; }


    [JsonProperty("use_images")]
    public bool UseImages { get; set; }
}
