using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "SitemapXmlResponse")]
public partial record SitemapXmlResponse : BaseNopDto
{
    [JsonProperty("site_map_xml")]
    public string SiteMapXml { get; set; }

    [JsonProperty("mime_type")]
    public string MimeType { get; set; }
}
