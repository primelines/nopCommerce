using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "BoardsIndex")]
public partial record BoardsIndexDto : BaseNopDto
{
    public BoardsIndexDto()
    {
        ForumGroups = new List<ForumGroupDto>();
    }


    [JsonProperty("forum_groups")]
    public IList<ForumGroupDto> ForumGroups { get; set; }
}
