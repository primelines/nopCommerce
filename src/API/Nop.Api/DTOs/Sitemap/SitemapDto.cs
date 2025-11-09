using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Sitemap;

[JsonObject(Title = "Sitemap")]
public partial record SitemapDto : BaseNopDto
{
    #region Ctor

    public SitemapDto()
    {
        Items = new List<SitemapItemModel>();
        Page = new SitemapPageDto();
    }

    #endregion

    #region Properties


    [JsonProperty("items")]
    public List<SitemapItemModel> Items { get; set; }


    [JsonProperty("page")]
    public SitemapPageDto Page { get; set; }

    #endregion

    #region Nested classes

    public partial record SitemapItemModel
    {

    [JsonProperty("group_title")]
        public string GroupTitle { get; set; }

    [JsonProperty("url")]
        public string Url { get; set; }

    [JsonProperty("name")]
        public string Name { get; set; }
    }

    #endregion
}
