using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a specification filter model
/// </summary>
[JsonObject(Title = "SpecificationFilter")]
public partial record SpecificationFilterDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether filtering is enabled
    /// </summary>

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the filtrable specification attributes
    /// </summary>

    [JsonProperty("attributes")]
    public IList<SpecificationAttributeFilterDto> Attributes { get; set; }

    #endregion

    #region Ctor

    public SpecificationFilterDto()
    {
        Attributes = new List<SpecificationAttributeFilterDto>();
    }

    #endregion
}
