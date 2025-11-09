using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutPickupPoint")]
public partial record CheckoutPickupPointDto : BaseNopDto
{

    [JsonProperty("id")]
    public string Id { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("provider_system_name")]
    public string ProviderSystemName { get; set; }


    [JsonProperty("address")]
    public string Address { get; set; }


    [JsonProperty("city")]
    public string City { get; set; }


    [JsonProperty("county")]
    public string County { get; set; }


    [JsonProperty("state_name")]
    public string StateName { get; set; }


    [JsonProperty("country_name")]
    public string CountryName { get; set; }


    [JsonProperty("zip_postal_code")]
    public string ZipPostalCode { get; set; }


    [JsonProperty("latitude")]
    public decimal? Latitude { get; set; }


    [JsonProperty("longitude")]
    public decimal? Longitude { get; set; }


    [JsonProperty("pickup_fee")]
    public string PickupFee { get; set; }


    [JsonProperty("opening_hours")]
    public string OpeningHours { get; set; }


    [JsonProperty("address_line")]
    public string AddressLine { get; set; }


    [JsonProperty("is_pre_selected")]
    public bool IsPreSelected { get; set; }
}
