using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a products price range filter model
/// </summary>
[JsonObject(Title = "PriceRangeFilter")]
public partial record PriceRangeFilterDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether filtering is enabled
    /// </summary>

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the selected price range
    /// </summary>

    [JsonProperty("selected_price_range")]
    public PriceRangeDto SelectedPriceRange { get; set; }

    /// <summary>
    /// Gets or sets the available price range
    /// </summary>

    [JsonProperty("available_price_range")]
    public PriceRangeDto AvailablePriceRange { get; set; }

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    public PriceRangeFilterDto()
    {
        SelectedPriceRange = new PriceRangeDto();
        AvailablePriceRange = new PriceRangeDto();
    }

    #endregion
}
