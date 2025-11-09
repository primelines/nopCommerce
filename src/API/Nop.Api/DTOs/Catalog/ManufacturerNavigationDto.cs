using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ManufacturerNavigation")]
public partial record ManufacturerNavigationDto : BaseNopDto
{
    public ManufacturerNavigationDto()
    {
        Manufacturers = new List<ManufacturerBriefInfoDto>();
    }


    [JsonProperty("manufacturers")]
    public IList<ManufacturerBriefInfoDto> Manufacturers { get; set; }


    [JsonProperty("total_manufacturers")]
    public int TotalManufacturers { get; set; }
}

[JsonObject(Title = "ManufacturerBriefInfo")]
public partial record ManufacturerBriefInfoDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
}
