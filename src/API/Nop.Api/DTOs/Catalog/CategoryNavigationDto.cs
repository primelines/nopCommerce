using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CategoryNavigation")]
public partial record CategoryNavigationDto : BaseNopDto
{
    public CategoryNavigationDto()
    {
        Categories = new List<CategorySimpleDto>();
    }


    [JsonProperty("current_category_id")]
    public int CurrentCategoryId { get; set; }

    [JsonProperty("categories")]
    public List<CategorySimpleDto> Categories { get; set; }

}
