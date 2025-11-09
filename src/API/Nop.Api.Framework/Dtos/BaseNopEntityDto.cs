
using Newtonsoft.Json;

namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents base nopCommerce entity model
/// </summary>
public partial record BaseNopEntityDto : BaseNopDto
{
    /// <summary>
    /// Gets or sets model identifier
    /// </summary>
    [JsonProperty("id")]
    public virtual int Id { get; set; }
}