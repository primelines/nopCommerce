using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "EstimateShipping")]
public partial record EstimateShippingDto : BaseNopDto
{
    public EstimateShippingDto()
    {
        AvailableCountries = new List<SelectListItemDto>();
        AvailableStates = new List<SelectListItemDto>();
    }


    [JsonProperty("request_delay")]
    public int RequestDelay { get; set; }


    [JsonProperty("enabled")]
    public bool Enabled { get; set; }


    [JsonProperty("country_id")]
    public int? CountryId { get; set; }

    [JsonProperty("state_province_id")]
    public int? StateProvinceId { get; set; }

    [JsonProperty("zip_postal_code")]
    public string ZipPostalCode { get; set; }

    [JsonProperty("use_city")]
    public bool UseCity { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }


    [JsonProperty("available_countries")]
    public IList<SelectListItemDto> AvailableCountries { get; set; }

    [JsonProperty("available_states")]
    public IList<SelectListItemDto> AvailableStates { get; set; }
}

[JsonObject(Title = "EstimateShippingResult")]
public partial record EstimateShippingResultDto : BaseNopDto
{
    public EstimateShippingResultDto()
    {
        ShippingOptions = new List<ShippingOptionDto>();
        Errors = new List<string>();
    }


    [JsonProperty("shipping_options")]
    public IList<ShippingOptionDto> ShippingOptions { get; set; }

    public bool Success => !Errors.Any();


    [JsonProperty("errors")]
    public IList<string> Errors { get; set; }

    #region Nested Classes

    public partial record ShippingOptionDto : BaseNopDto
    {

    [JsonProperty("name")]
        public string Name { get; set; }


    [JsonProperty("shipping_rate_computation_method_system_name")]
        public string ShippingRateComputationMethodSystemName { get; set; }


    [JsonProperty("description")]
        public string Description { get; set; }


    [JsonProperty("price")]
        public string Price { get; set; }


    [JsonProperty("rate")]
        public decimal Rate { get; set; }


    [JsonProperty("delivery_date_format")]
        public string DeliveryDateFormat { get; set; }


    [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }


    [JsonProperty("selected")]
        public bool Selected { get; set; }
    }

    #endregion
}
