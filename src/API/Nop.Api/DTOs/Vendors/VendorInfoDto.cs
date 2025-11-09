using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Vendors;

[JsonObject(Title = "VendorInfo")]
public partial record VendorInfoDto : BaseNopDto
{
    public VendorInfoDto()
    {
        VendorAttributes = new List<VendorAttributeDto>();
    }

    [NopResourceDisplayName("Account.VendorInfo.Name")]

    [JsonProperty("name")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.VendorInfo.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Account.VendorInfo.Description")]

    [JsonProperty("description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Account.VendorInfo.Picture")]

    [JsonProperty("picture_url")]
    public string PictureUrl { get; set; }


    [JsonProperty("vendor_attributes")]
    public IList<VendorAttributeDto> VendorAttributes { get; set; }
}
