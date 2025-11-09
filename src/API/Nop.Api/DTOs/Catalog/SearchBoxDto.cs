using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "SearchBox")]
public partial record SearchBoxDto : BaseNopDto
{

    [JsonProperty("auto_complete_enabled")]
    public bool AutoCompleteEnabled { get; set; }

    [JsonProperty("show_product_images_in_search_auto_complete")]
    public bool ShowProductImagesInSearchAutoComplete { get; set; }

    [JsonProperty("search_term_minimum_length")]
    public int SearchTermMinimumLength { get; set; }

    [JsonProperty("show_search_box")]
    public bool ShowSearchBox { get; set; }
}
