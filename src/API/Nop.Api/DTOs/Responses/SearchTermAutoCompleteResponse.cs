using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "SearchTermAutoCompleteResponse")]
public partial record SearchTermAutoCompleteResponse : BaseNopDto
{
    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("producturl")]
    public string Producturl { get; set; }

    [JsonProperty("productpictureurl")]
    public string Productpictureurl { get; set; }

    [JsonProperty("showlinktoresultsearch")]
    public bool Showlinktoresultsearch { get; set; }
}
