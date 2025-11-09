using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutPickupPoints")]
public partial record CheckoutPickupPointsDto : BaseNopDto
{
    public CheckoutPickupPointsDto()
    {
        Warnings = new List<string>();
        PickupPoints = new List<CheckoutPickupPointDto>();
    }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }


    [JsonProperty("pickup_points")]
    public IList<CheckoutPickupPointDto> PickupPoints { get; set; }

    [JsonProperty("allow_pickup_in_store")]
    public bool AllowPickupInStore { get; set; }

    [JsonProperty("pickup_in_store")]
    public bool PickupInStore { get; set; }

    [JsonProperty("pickup_in_store_only")]
    public bool PickupInStoreOnly { get; set; }

    [JsonProperty("display_pickup_points_on_map")]
    public bool DisplayPickupPointsOnMap { get; set; }

    [JsonProperty("google_maps_api_key")]
    public string GoogleMapsApiKey { get; set; }
}
