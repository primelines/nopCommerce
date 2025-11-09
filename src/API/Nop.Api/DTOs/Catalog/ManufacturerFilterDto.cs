using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

/// <summary>
/// Represents a manufacturer filter model
/// </summary>
[JsonObject(Title = "ManufacturerFilter")]
public partial record ManufacturerFilterDto : BaseNopDto
{
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether filtering is enabled
    /// </summary>

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the filtrable manufacturers
    /// </summary>

    [JsonProperty("manufacturers")]
    public IList<SelectListItemDto> Manufacturers { get; set; }

    #endregion

    #region Ctor

    public ManufacturerFilterDto()
    {
        Manufacturers = new List<SelectListItemDto>();
    }

    #endregion
}
