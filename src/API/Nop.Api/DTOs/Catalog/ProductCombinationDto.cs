using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a product combination model
/// </summary>
[JsonObject(Title = "ProductCombination")]
public partial record ProductCombinationDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the attributes
    /// </summary>

    [JsonProperty("attributes")]
    public IList<ProductAttributeDto> Attributes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to the combination have stock
    /// </summary>

    [JsonProperty("in_stock")]
    public bool InStock { get; set; }

    #endregion

    #region Ctor

    public ProductCombinationDto()
    {
        Attributes = new List<ProductAttributeDto>();
    }

    #endregion
}
