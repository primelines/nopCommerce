using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "HomepageNewsItems")]
public partial record HomepageNewsItemsDto : BaseNopDto
{
    public HomepageNewsItemsDto()
    {
        NewsItems = new List<NewsItemDto>();
    }


    [JsonProperty("working_language_id")]
    public int WorkingLanguageId { get; set; }

    [JsonProperty("news_items")]
    public IList<NewsItemDto> NewsItems { get; set; }
}
