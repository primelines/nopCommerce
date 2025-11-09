using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "PostVoteResponse")]
public partial record PostVoteResponse : BaseNopDto
{

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    [JsonProperty("is_up")]
    public bool IsUp { get; set; }

}
