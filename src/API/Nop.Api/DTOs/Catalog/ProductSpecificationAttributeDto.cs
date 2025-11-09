using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a specification attribute model
/// </summary>
[JsonObject(Title = "ProductSpecificationAttribute")]
public partial record ProductSpecificationAttributeDto : BaseNopEntityDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the name
    /// </summary>

    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the values
    /// </summary>

    [JsonProperty("values")]
    public IList<ProductSpecificationAttributeValueDto> Values { get; set; }

    #endregion

    #region Ctor

    public ProductSpecificationAttributeDto()
    {
        Values = new List<ProductSpecificationAttributeValueDto>();
    }

    #endregion
}
