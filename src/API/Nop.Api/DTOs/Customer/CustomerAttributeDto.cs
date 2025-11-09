using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerAttribute")]
public partial record CustomerAttributeDto : BaseNopEntityDto
{
    public CustomerAttributeDto()
    {
        Values = new List<CustomerAttributeValueDto>();
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
    public IList<CustomerAttributeValueDto> Values { get; set; }

}

[JsonObject(Title = "CustomerAttributeValue")]
public partial record CustomerAttributeValueDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("is_pre_selected")]
    public bool IsPreSelected { get; set; }
}
