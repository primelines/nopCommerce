using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Videos;

[JsonObject(Title = "Video")]
public partial record VideoDto : BaseNopDto
{
    public int Id { get; set; }

    public int VendorId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("caption")]
    public string Description { get; set; }

    [JsonProperty("video_url")]
    public string VideoUrl { get; set; }

    [JsonProperty("thumb_url")]
    public string ThumbnailUrl { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    [JsonProperty("views")]
    public int Views { get; set; }

    [JsonProperty("likes")]
    public int Likes { get; set; }

    [JsonProperty("shares")]
    public int Shares { get; set; }

    [JsonProperty("comments")]
    public int Comments { get; set; }

    [JsonProperty("is_liked")]
    public bool IsLikedByCurrentCustomer { get; set; }

    [JsonProperty("tags")]
    public IList<string> Tags { get; set; } = new List<string>();

    [JsonProperty("products")]
    public IList<ProductBriefDto> Products { get; set; } = new List<ProductBriefDto>();

    [JsonProperty("vendor")]
    public VendorBriefDto Vendor { get; set; } = new VendorBriefDto();
}
