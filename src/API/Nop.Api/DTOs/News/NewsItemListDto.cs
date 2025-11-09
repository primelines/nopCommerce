using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "NewsItemList")]
public partial record NewsItemListDto : BaseNopDto
{
    public NewsItemListDto()
    {
        PagingFilteringContext = new NewsPagingFilteringDto();
        NewsItems = new List<NewsItemDto>();
    }


    [JsonProperty("working_language_id")]
    public int WorkingLanguageId { get; set; }

    [JsonProperty("paging_filtering_context")]
    public NewsPagingFilteringDto PagingFilteringContext { get; set; }

    [JsonProperty("news_items")]
    public IList<NewsItemDto> NewsItems { get; set; }
}
