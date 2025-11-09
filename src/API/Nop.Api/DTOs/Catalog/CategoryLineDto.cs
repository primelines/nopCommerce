using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CategoryLine")]
public partial record CategoryLineDto : BaseNopDto
{

    [JsonProperty("current_category_id")]
    public int CurrentCategoryId { get; set; }

    [JsonProperty("category")]
    public CategorySimpleDto Category { get; set; }
}
