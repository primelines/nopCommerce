using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a price range model
/// </summary>
[JsonObject(Title = "PriceRange")]
public partial record PriceRangeDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets the "from" price
    /// </summary>

    [JsonProperty("from")]
    public decimal? From { get; set; }

    /// <summary>
    /// Gets or sets the "to" price
    /// </summary>

    [JsonProperty("to")]
    public decimal? To { get; set; }

    #endregion
}
