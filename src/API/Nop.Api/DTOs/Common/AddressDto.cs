using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Common;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Address")]
public partial record AddressDto : BaseNopEntityDto
{
    public AddressDto()
    {
        AvailableCountries = new List<SelectListItemDto>();
        AvailableStates = new List<SelectListItemDto>();
        CustomAddressAttributes = new List<AddressAttributeDto>();
        AddressFields = new KeyValuePair<AddressField, string>[7];
    }


    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }
    [DataType(DataType.EmailAddress)]

    [JsonProperty("email")]
    public string Email { get; set; }



    [JsonProperty("company_enabled")]
    public bool CompanyEnabled { get; set; }

    [JsonProperty("company_required")]
    public bool CompanyRequired { get; set; }

    [JsonProperty("company")]
    public string Company { get; set; }


    [JsonProperty("country_enabled")]
    public bool CountryEnabled { get; set; }

    [JsonProperty("country_id")]
    public int? CountryId { get; set; }

    [JsonProperty("country_name")]
    public string CountryName { get; set; }


    [JsonProperty("default_country_id")]
    public int? DefaultCountryId { get; set; }


    [JsonProperty("state_province_enabled")]
    public bool StateProvinceEnabled { get; set; }

    [JsonProperty("state_province_id")]
    public int? StateProvinceId { get; set; }

    [JsonProperty("state_province_name")]
    public string StateProvinceName { get; set; }


    [JsonProperty("county_enabled")]
    public bool CountyEnabled { get; set; }

    [JsonProperty("county_required")]
    public bool CountyRequired { get; set; }

    [JsonProperty("county")]
    public string County { get; set; }


    [JsonProperty("city_enabled")]
    public bool CityEnabled { get; set; }

    [JsonProperty("city_required")]
    public bool CityRequired { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }


    [JsonProperty("street_address_enabled")]
    public bool StreetAddressEnabled { get; set; }

    [JsonProperty("street_address_required")]
    public bool StreetAddressRequired { get; set; }

    [JsonProperty("address1")]
    public string Address1 { get; set; }


    [JsonProperty("street_address2_enabled")]
    public bool StreetAddress2Enabled { get; set; }

    [JsonProperty("street_address2_required")]
    public bool StreetAddress2Required { get; set; }

    [JsonProperty("address2")]
    public string Address2 { get; set; }


    [JsonProperty("zip_postal_code_enabled")]
    public bool ZipPostalCodeEnabled { get; set; }

    [JsonProperty("zip_postal_code_required")]
    public bool ZipPostalCodeRequired { get; set; }

    [JsonProperty("zip_postal_code")]
    public string ZipPostalCode { get; set; }


    [JsonProperty("phone_enabled")]
    public bool PhoneEnabled { get; set; }

    [JsonProperty("phone_required")]
    public bool PhoneRequired { get; set; }

    [DataType(DataType.PhoneNumber)]
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }


    [JsonProperty("fax_enabled")]
    public bool FaxEnabled { get; set; }

    [JsonProperty("fax_required")]
    public bool FaxRequired { get; set; }

    [JsonProperty("fax_number")]
    public string FaxNumber { get; set; }


    [JsonProperty("address_line")]
    public string AddressLine { get; set; }

    [JsonIgnore]
    public KeyValuePair<AddressField, string>[] AddressFields { get; set; }


    [JsonProperty("available_countries")]
    public IList<SelectListItemDto> AvailableCountries { get; set; }

    [JsonProperty("available_states")]
    public IList<SelectListItemDto> AvailableStates { get; set; }


    [JsonProperty("formatted_custom_address_attributes")]
    public string FormattedCustomAddressAttributes { get; set; }

    [JsonProperty("custom_address_attributes")]
    public IList<AddressAttributeDto> CustomAddressAttributes { get; set; }

    public Address ToEntity(Address destination = null)
    {
        destination ??= new Address();
        
        destination.Id = Id;
        destination.FirstName = FirstName;
        destination.LastName = LastName;
        destination.Email = Email;
        destination.Company = Company;
        destination.CountryId = CountryId == 0 ? null : CountryId;
        destination.StateProvinceId = StateProvinceId == 0 ? null : StateProvinceId;
        destination.County = County;
        destination.City = City;
        destination.Address1 = Address1;
        destination.Address2 = Address2;
        destination.ZipPostalCode = ZipPostalCode;
        destination.PhoneNumber = PhoneNumber;
        destination.FaxNumber = FaxNumber;

        return destination;
    }
}
