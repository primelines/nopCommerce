using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "ShipmentDetails")]
public partial record ShipmentDetailsDto : BaseNopEntityDto
{
    public ShipmentDetailsDto()
    {
        ShipmentStatusEvents = new List<ShipmentStatusEventModel>();
        Items = new List<ShipmentItemDto>();
    }


    [JsonProperty("tracking_number")]
    public string TrackingNumber { get; set; }

    [JsonProperty("tracking_number_url")]
    public string TrackingNumberUrl { get; set; }

    [JsonProperty("shipped_date")]
    public DateTime? ShippedDate { get; set; }

    [JsonProperty("ready_for_pickup_date")]
    public DateTime? ReadyForPickupDate { get; set; }

    [JsonProperty("delivery_date")]
    public DateTime? DeliveryDate { get; set; }

    [JsonProperty("shipment_status_events")]
    public IList<ShipmentStatusEventModel> ShipmentStatusEvents { get; set; }

    [JsonProperty("show_sku")]
    public bool ShowSku { get; set; }

    [JsonProperty("items")]
    public IList<ShipmentItemDto> Items { get; set; }


    [JsonProperty("order")]
    public OrderDetailsDto Order { get; set; }

    #region Nested Classes

    public partial record ShipmentItemDto : BaseNopEntityDto
    {

    [JsonProperty("sku")]
        public string Sku { get; set; }

    [JsonProperty("product_id")]
        public int ProductId { get; set; }

    [JsonProperty("product_name")]
        public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }

    [JsonProperty("attribute_info")]
        public string AttributeInfo { get; set; }

    [JsonProperty("rental_info")]
        public string RentalInfo { get; set; }


    [JsonProperty("quantity_ordered")]
        public int QuantityOrdered { get; set; }

    [JsonProperty("quantity_shipped")]
        public int QuantityShipped { get; set; }
    }

    public partial record ShipmentStatusEventModel : BaseNopDto
    {

    [JsonProperty("status")]
        public string Status { get; set; }

    [JsonProperty("event_name")]
        public string EventName { get; set; }

    [JsonProperty("location")]
        public string Location { get; set; }

    [JsonProperty("country")]
        public string Country { get; set; }

    [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }

    #endregion
}
