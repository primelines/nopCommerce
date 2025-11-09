using Newtonsoft.Json;
using Nop.Api.Infrastructure;

namespace Nop.Api.DTOs.Common;

/// <summary>
/// record that has a slug and page for route values. Used for Topic (posts) and 
/// Forum (topics) pagination
/// </summary>
[JsonObject(Title = "SlugRouteValues")]
public partial record SlugRouteValues : BaseRouteValues
{

    [JsonProperty("id")]
    public int Id { get; set; }


    [JsonProperty("slug")]
    public string Slug { get; set; }
}
