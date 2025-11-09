using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Vendors;

[JsonObject(Title = "VendorAttribute")]
public partial record VendorAttributeDto : BaseNopEntityDto
{
    public VendorAttributeDto()
    {
        Values = new List<VendorAttributeValueDto>();
    }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Default value for textboxes
    /// </summary>

    [JsonProperty("default_value")]
    public string DefaultValue { get; set; }


    [JsonProperty("attribute_control_type")]
    public AttributeControlType AttributeControlType { get; set; }


    [JsonProperty("values")]
    public IList<VendorAttributeValueDto> Values { get; set; }

}

[JsonObject(Title = "VendorAttributeValue")]
public partial record VendorAttributeValueDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("is_pre_selected")]
    public bool IsPreSelected { get; set; }
}
