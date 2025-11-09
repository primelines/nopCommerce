using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "AddressAttribute")]
public partial record AddressAttributeDto : BaseNopEntityDto
{
    public AddressAttributeDto()
    {
        Values = new List<AddressAttributeValueDto>();
    }


    [JsonProperty("control_id")]
    public string ControlId { get; set; }


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
    public IList<AddressAttributeValueDto> Values { get; set; }
}

[JsonObject(Title = "AddressAttributeValue")]
public partial record AddressAttributeValueDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("is_pre_selected")]
    public bool IsPreSelected { get; set; }
}
