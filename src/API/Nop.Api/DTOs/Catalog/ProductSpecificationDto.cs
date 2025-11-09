using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a product specification model
/// </summary>
[JsonObject(Title = "ProductSpecification")]
public partial record ProductSpecificationDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the grouped specification attribute models
    /// </summary>

    [JsonProperty("groups")]
    public IList<ProductSpecificationAttributeGroupDto> Groups { get; set; }

    #endregion

    #region Ctor

    public ProductSpecificationDto()
    {
        Groups = new List<ProductSpecificationAttributeGroupDto>();
    }

    #endregion
}
