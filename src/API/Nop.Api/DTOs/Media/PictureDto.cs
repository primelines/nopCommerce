using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Media;

[JsonObject(Title = "Picture")]
public partial record PictureDto : BaseNopEntityDto
{

    [JsonProperty("image_url")]
    public string ImageUrl { get; set; }


    [JsonProperty("thumb_image_url")]
    public string ThumbImageUrl { get; set; }


    [JsonProperty("full_size_image_url")]
    public string FullSizeImageUrl { get; set; }


    [JsonProperty("title")]
    public string Title { get; set; }


    [JsonProperty("alternate_text")]
    public string AlternateText { get; set; }
}
