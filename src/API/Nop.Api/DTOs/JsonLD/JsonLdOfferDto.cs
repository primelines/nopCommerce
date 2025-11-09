using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.JsonLD;

[JsonObject(Title = "JsonLdOffer")]
public record JsonLdOfferDto : JsonLdDto
{
    #region Properties


    [JsonProperty("@type")]
    public static string Type => "Offer";

    [JsonProperty("url")]
    public string Url { get; set; }


    [JsonProperty("availability")]
    public string Availability { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }


    [JsonProperty("price_currency")]
    public string PriceCurrency { get; set; }


    [JsonProperty("price_valid_until")]
    public DateTime? PriceValidUntil { get; set; }

    #endregion
}
