using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using System.Collections.Generic;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ProductDetailsAttributeChangeResponse")]
public partial record ProductDetailsAttributeChangeResponse : BaseNopDto
{
    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    [JsonProperty("gtin")]
    public string Gtin { get; set; }

    [JsonProperty("mpn")]
    public string Mpn { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("base_price_pangv")]
    public string BasePricePangv { get; set; }

    [JsonProperty("stock_availability")]
    public string StockAvailability { get; set; }

    [JsonProperty("enabledattributemappingids")]
    public IList<int> Enabledattributemappingids { get; set; }

    [JsonProperty("disabledattributemappingids")]
    public IList<int> Disabledattributemappingids { get; set; }

    [JsonProperty("picture_full_size_url")]
    public string PictureFullSizeUrl { get; set; }

    [JsonProperty("picture_default_size_url")]
    public string PictureDefaultSizeUrl { get; set; }

    [JsonProperty("is_free_shipping")]
    public bool IsFreeShipping { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
