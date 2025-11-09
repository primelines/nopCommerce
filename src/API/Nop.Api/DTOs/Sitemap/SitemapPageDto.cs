using Newtonsoft.Json;
using Nop.Api.Framework.UI.Paging;

namespace Nop.Api.DTOs.Sitemap;

[JsonObject(Title = "SitemapPage")]
public partial record SitemapPageDto : BasePageableDto
{
}
