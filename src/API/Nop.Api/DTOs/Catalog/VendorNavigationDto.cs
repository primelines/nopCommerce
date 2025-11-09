using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "VendorNavigation")]
public partial record VendorNavigationDto : BaseNopDto
{
    public VendorNavigationDto()
    {
        Vendors = new List<VendorBriefInfoDto>();
    }


    [JsonProperty("vendors")]
    public IList<VendorBriefInfoDto> Vendors { get; set; }


    [JsonProperty("total_vendors")]
    public int TotalVendors { get; set; }
}

[JsonObject(Title = "VendorBriefInfo")]
public partial record VendorBriefInfoDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("se_name")]
    public string SeName { get; set; }
}
