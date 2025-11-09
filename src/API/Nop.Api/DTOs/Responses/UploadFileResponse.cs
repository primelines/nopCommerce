using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using System;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "UploadFileResponse")]
public partial record UploadFileResponse : BaseNopDto
{


    [JsonProperty("success")]
    public bool Success { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }


    [JsonProperty("download_url")]
    public string DownloadUrl { get; set; }


    [JsonProperty("download_guid")]
    public Guid DownloadGuid { get; set; }


}
