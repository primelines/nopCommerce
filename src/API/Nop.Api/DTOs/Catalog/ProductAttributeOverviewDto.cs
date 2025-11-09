using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;


[JsonObject(Title = "ProductAttributeOverview")]
public partial record ProductAttributeOverviewDto : BaseNopEntityDto
{
    public ProductAttributeOverviewDto()
    {
        AllowedFileExtensions = new List<string>();
        Values = new List<ProductAttributeValueDto>();
    }


    [JsonProperty("product_id")]
    public int ProductId { get; set; }


    [JsonProperty("product_attribute_id")]
    public int ProductAttributeId { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("text_prompt")]
    public string TextPrompt { get; set; }


    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Default value for textboxes
    /// </summary>

    [JsonProperty("default_value")]
    public string DefaultValue { get; set; }
    /// <summary>
    /// Selected day value for datepicker
    /// </summary>

    [JsonProperty("selected_day")]
    public int? SelectedDay { get; set; }
    /// <summary>
    /// Selected month value for datepicker
    /// </summary>

    [JsonProperty("selected_month")]
    public int? SelectedMonth { get; set; }
    /// <summary>
    /// Selected year value for datepicker
    /// </summary>

    [JsonProperty("selected_year")]
    public int? SelectedYear { get; set; }

    /// <summary>
    /// A value indicating whether this attribute depends on some other attribute
    /// </summary>

    [JsonProperty("has_condition")]
    public bool HasCondition { get; set; }

    /// <summary>
    /// Allowed file extensions for customer uploaded files
    /// </summary>

    [JsonProperty("allowed_file_extensions")]
    public IList<string> AllowedFileExtensions { get; set; }


    [JsonProperty("attribute_control_type")]
    public AttributeControlType AttributeControlType { get; set; }


    [JsonProperty("values")]
    public IList<ProductAttributeValueDto> Values { get; set; }
}
