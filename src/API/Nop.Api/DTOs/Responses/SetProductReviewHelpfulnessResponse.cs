using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "SetProductReviewHelpfulnessResponse")]
public partial record SetProductReviewHelpfulnessResponse : BaseNopDto
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("total_yes")]
    public int TotalYes { get; set; }

    [JsonProperty("total_no")]
    public int TotalNo { get; set; }
}
