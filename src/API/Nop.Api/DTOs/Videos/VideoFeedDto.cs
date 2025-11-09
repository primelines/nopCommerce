using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Videos;

[JsonObject(Title = "VideoFeed")]
public partial record VideoFeedDto : BaseNopDto
{
    [JsonProperty("page")]
    public int PageIndex { get; set; }

    [JsonProperty("page_size")]
    public int PageSize { get; set; }

    [JsonProperty("total_items")]
    public int TotalItems { get; set; }

    [JsonProperty("items")]
    public IList<VideoDto> Items { get; set; } = new List<VideoDto>();
}
