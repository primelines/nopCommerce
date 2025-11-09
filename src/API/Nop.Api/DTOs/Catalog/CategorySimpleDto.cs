using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CategorySimple")]
public partial record CategorySimpleDto : BaseNopEntityDto
{
    public CategorySimpleDto()
    {
        SubCategories = new List<CategorySimpleDto>();
    }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("number_of_products")]
    public int? NumberOfProducts { get; set; }


    //[JsonProperty("include_in_top_menu")]
    //public bool IncludeInTopMenu { get; set; }


    [JsonProperty("sub_categories")]
    public List<CategorySimpleDto> SubCategories { get; set; }


    [JsonProperty("have_sub_categories")]
    public bool HaveSubCategories { get; set; }


    [JsonProperty("route")]
    public string Route { get; set; }
}
