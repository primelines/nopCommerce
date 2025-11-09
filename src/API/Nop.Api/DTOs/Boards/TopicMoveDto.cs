using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "TopicMove")]
public partial record TopicMoveDto : BaseNopEntityDto
{
    public TopicMoveDto()
    {
        ForumList = new List<SelectListItemDto>();
    }


    [JsonProperty("forum_selected")]
    public int ForumSelected { get; set; }

    [JsonProperty("topic_se_name")]
    public string TopicSeName { get; set; }


    [JsonProperty("forum_list")]
    public IEnumerable<SelectListItemDto> ForumList { get; set; }
}
