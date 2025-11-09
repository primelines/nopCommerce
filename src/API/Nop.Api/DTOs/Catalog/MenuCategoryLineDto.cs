using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;


public partial record MenuCategoryLineDto : BaseNopDto
{

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("responsive_mobile_menu")]
    public bool ResponsiveMobileMenu { get; set; }

    [JsonProperty("category")]
    public CategorySimpleDto Category { get; set; }
}