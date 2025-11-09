using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "RobotsTextFileResponse")]
public partial record RobotsTextFileResponse : BaseNopDto
{
    [JsonProperty("robots_file_content")]
    public string RobotsFileContent { get; set; }

    [JsonProperty("mime_type")]
    public string MimeType { get; set; }


}
