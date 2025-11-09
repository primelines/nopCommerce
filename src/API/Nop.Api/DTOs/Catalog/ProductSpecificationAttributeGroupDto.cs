using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a grouped product specification attribute model
/// </summary>
[JsonObject(Title = "ProductSpecificationAttributeGroup")]
public partial record ProductSpecificationAttributeGroupDto : BaseNopEntityDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the specification attribute group name
    /// </summary>

    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the specification attribute group attributes
    /// </summary>

    [JsonProperty("attributes")]
    public IList<ProductSpecificationAttributeDto> Attributes { get; set; }

    #endregion

    #region Ctor

    public ProductSpecificationAttributeGroupDto()
    {
        Attributes = new List<ProductSpecificationAttributeDto>();
    }

    #endregion
}
