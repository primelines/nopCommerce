using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Videos;

[JsonObject(Title = "ProductBrief")]
public partial record ProductBriefDto : BaseNopDto
{
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string SeName { get; set; }

    [JsonProperty("main_image_url")]
    public string PictureUrl { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("old_price")]
    public string OldPrice { get; set; }

    [JsonProperty("allow_cart")]
    public bool AllowAddToCart { get; set; }

    [JsonProperty("allow_preorder")]
    public bool AvailableForPreOrder { get; set; }
}
