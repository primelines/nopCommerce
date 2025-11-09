using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a product specification attribute value model
/// </summary>
[JsonObject(Title = "ProductSpecificationAttributeValue")]
public partial record ProductSpecificationAttributeValueDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the attribute type id
    /// </summary>

    [JsonProperty("attribute_type_id")]
    public int AttributeTypeId { get; set; }

    /// <summary>
    /// Gets or sets the value raw. This value is already HTML encoded
    /// </summary>

    [JsonProperty("value_raw")]
    public string ValueRaw { get; set; }

    /// <summary>
    /// Gets or sets the option color (if specified). Used to display color squares
    /// </summary>

    [JsonProperty("color_squares_rgb")]
    public string ColorSquaresRgb { get; set; }

    #endregion
}
