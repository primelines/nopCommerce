using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "ShipmentBrief")]
public partial record ShipmentBriefDto : BaseNopEntityDto
{

    [JsonProperty("tracking_number")]
    public string TrackingNumber { get; set; }

    [JsonProperty("shipped_date")]
    public DateTime? ShippedDate { get; set; }

    [JsonProperty("ready_for_pickup_date")]
    public DateTime? ReadyForPickupDate { get; set; }

    [JsonProperty("delivery_date")]
    public DateTime? DeliveryDate { get; set; }
}

