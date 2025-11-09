using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a specification attribute filter model
/// </summary>
[JsonObject(Title = "SpecificationAttributeFilter")]
public partial record SpecificationAttributeFilterDto : BaseNopEntityDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the specification attribute name
    /// </summary>

    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the values
    /// </summary>

    [JsonProperty("values")]
    public IList<SpecificationAttributeValueFilterDto> Values { get; set; }

    #endregion

    #region Ctor

    public SpecificationAttributeFilterDto()
    {
        Values = new List<SpecificationAttributeValueFilterDto>();
    }

    #endregion
}
