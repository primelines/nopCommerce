using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Videos;

[JsonObject(Title = "VendorBrief")]
public partial record VendorBriefDto : BaseNopDto
{
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string SeName { get; set; }

    [JsonProperty("profile_url")]
    public string PictureUrl { get; set; }

    [JsonProperty("is_followed")]
    public bool IsFollowedByCurrentCustomer { get; set; }
}
