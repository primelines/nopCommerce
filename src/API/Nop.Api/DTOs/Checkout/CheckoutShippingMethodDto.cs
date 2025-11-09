using Newtonsoft.Json;
using Nop.Core.Domain.Shipping;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutShippingMethod")]
public partial record CheckoutShippingMethodDto : BaseNopDto
{
    public CheckoutShippingMethodDto()
    {
        ShippingMethods = new List<ShippingMethodDto>();
        Warnings = new List<string>();
    }


    [JsonProperty("shipping_methods")]
    public IList<ShippingMethodDto> ShippingMethods { get; set; }


    [JsonProperty("notify_customer_about_shipping_from_multiple_locations")]
    public bool NotifyCustomerAboutShippingFromMultipleLocations { get; set; }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }


    [JsonProperty("display_pickup_in_store")]
    public bool DisplayPickupInStore { get; set; }

    [JsonProperty("pickup_points")]
    public CheckoutPickupPointsDto PickupPoints { get; set; }

    #region Nested classes

    public partial record ShippingMethodDto : BaseNopDto
    {

    [JsonProperty("shipping_rate_computation_method_system_name")]
        public string ShippingRateComputationMethodSystemName { get; set; }

    [JsonProperty("name")]
        public string Name { get; set; }

    [JsonProperty("description")]
        public string Description { get; set; }

    [JsonProperty("fee")]
        public string Fee { get; set; }

    [JsonProperty("rate")]
        public decimal Rate { get; set; }

    [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }

    [JsonProperty("selected")]
        public bool Selected { get; set; }

    [JsonProperty("shipping_option")]
        public ShippingOption ShippingOption { get; set; }
    }

    #endregion
}
