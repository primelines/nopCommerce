using Newtonsoft.Json;
using Nop.Api.Framework.UI.Paging;

namespace Nop.Api.DTOs.News;

[JsonObject(Title = "NewsPagingFiltering")]
public partial record NewsPagingFilteringDto : BasePageableDto
{
}
