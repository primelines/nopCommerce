using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a specification attribute value filter model
/// </summary>
[JsonObject(Title = "SpecificationAttributeValueFilter")]
public partial record SpecificationAttributeValueFilterDto : BaseNopEntityDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the specification attribute option name
    /// </summary>

    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the specification attribute option color (RGB)
    /// </summary>

    [JsonProperty("color_squares_rgb")]
    public string ColorSquaresRgb { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the value is selected
    /// </summary>

    [JsonProperty("selected")]
    public bool Selected { get; set; }

    #endregion
}
