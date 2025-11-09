using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumGroup")]
public partial record ForumGroupDto : BaseNopDto
{
    public ForumGroupDto()
    {
        Forums = new List<ForumRowDto>();
    }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("forums")]
    public IList<ForumRowDto> Forums { get; set; }
}
