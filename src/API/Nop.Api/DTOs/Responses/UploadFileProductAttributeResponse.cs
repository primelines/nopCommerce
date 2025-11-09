using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "UploadFileProductAttributeResponse")]
public partial record UploadFileProductAttributeResponse : BaseNopDto
{


    [JsonProperty("success")]
    public bool Success { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }


    [JsonProperty("download_url")]
    public string DownloadUrl { get; set; }


    [JsonProperty("download_guid")]
    public string DownloadGuid { get; set; }

}
