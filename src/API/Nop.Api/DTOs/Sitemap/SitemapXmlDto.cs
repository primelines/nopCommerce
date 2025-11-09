using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Sitemap;

[JsonObject(Title = "SitemapXml")]
public partial record SitemapXmlDto : BaseNopDto
{

    [JsonProperty("sitemap_xml_path")]
    public string SitemapXmlPath { get; set; }
}
