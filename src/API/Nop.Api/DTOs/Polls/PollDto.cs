using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Polls;

[JsonObject(Title = "Poll")]
public partial record PollDto : BaseNopEntityDto
{
    public PollDto()
    {
        Answers = new List<PollAnswerDto>();
    }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("already_voted")]
    public bool AlreadyVoted { get; set; }


    [JsonProperty("total_votes")]
    public int TotalVotes { get; set; }


    [JsonProperty("answers")]
    public IList<PollAnswerDto> Answers { get; set; }
}

[JsonObject(Title = "PollAnswer")]
public partial record PollAnswerDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("number_of_votes")]
    public int NumberOfVotes { get; set; }


    [JsonProperty("percent_of_total_votes")]
    public double PercentOfTotalVotes { get; set; }
}
