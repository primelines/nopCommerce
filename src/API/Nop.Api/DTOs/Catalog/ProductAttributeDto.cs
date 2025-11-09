using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a product attribute model
/// </summary>
[JsonObject(Title = "ProductAttribute")]
public partial record ProductAttributeDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the attribute id
    /// </summary>

    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the value IDs of the attribute
    /// </summary>

    [JsonProperty("value_ids")]
    public IList<int> ValueIds { get; set; }

    #endregion

    #region Ctor

    public ProductAttributeDto()
    {
        ValueIds = new List<int>();
    }

    #endregion
}
