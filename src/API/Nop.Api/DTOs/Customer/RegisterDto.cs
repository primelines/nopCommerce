using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "Register")]
public partial record RegisterDto : BaseNopDto
{
    public RegisterDto()
    {
        AvailableTimeZones = new List<SelectListItemDto>();
        AvailableCountries = new List<SelectListItemDto>();
        AvailableStates = new List<SelectListItemDto>();
        CustomerAttributes = new List<CustomerAttributeDto>();
        GdprConsents = new List<GdprConsentDto>();
    }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.Fields.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }


    [JsonProperty("entering_email_twice")]
    public bool EnteringEmailTwice { get; set; }
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.Fields.ConfirmEmail")]

    [JsonProperty("confirm_email")]
    public string ConfirmEmail { get; set; }


    [JsonProperty("usernames_enabled")]
    public bool UsernamesEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.Username")]

    [JsonProperty("username")]
    public string Username { get; set; }


    [JsonProperty("check_username_availability_enabled")]
    public bool CheckUsernameAvailabilityEnabled { get; set; }

    [NoTrim]
    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.Fields.Password")]

    [JsonProperty("password")]
    public string Password { get; set; }

    [NoTrim]
    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.Fields.ConfirmPassword")]

    [JsonProperty("confirm_password")]
    public string ConfirmPassword { get; set; }

    //form fields & properties

    [JsonProperty("gender_enabled")]
    public bool GenderEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.Gender")]

    [JsonProperty("gender")]
    public string Gender { get; set; }


    [JsonProperty("neutral_gender_enabled")]
    public bool NeutralGenderEnabled { get; set; }


    [JsonProperty("first_name_enabled")]
    public bool FirstNameEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.FirstName")]

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("first_name_required")]
    public bool FirstNameRequired { get; set; }

    [JsonProperty("last_name_enabled")]
    public bool LastNameEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.LastName")]

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("last_name_required")]
    public bool LastNameRequired { get; set; }


    [JsonProperty("date_of_birth_enabled")]
    public bool DateOfBirthEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.DateOfBirth")]

    [JsonProperty("date_of_birth_day")]
    public int? DateOfBirthDay { get; set; }
    [NopResourceDisplayName("Account.Fields.DateOfBirth")]

    [JsonProperty("date_of_birth_month")]
    public int? DateOfBirthMonth { get; set; }
    [NopResourceDisplayName("Account.Fields.DateOfBirth")]

    [JsonProperty("date_of_birth_year")]
    public int? DateOfBirthYear { get; set; }

    [JsonProperty("date_of_birth_required")]
    public bool DateOfBirthRequired { get; set; }
    public DateTime? ParseDateOfBirth()
    {
        return CommonHelper.ParseDate(DateOfBirthYear, DateOfBirthMonth, DateOfBirthDay);
    }


    [JsonProperty("company_enabled")]
    public bool CompanyEnabled { get; set; }

    [JsonProperty("company_required")]
    public bool CompanyRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.Company")]

    [JsonProperty("company")]
    public string Company { get; set; }


    [JsonProperty("street_address_enabled")]
    public bool StreetAddressEnabled { get; set; }

    [JsonProperty("street_address_required")]
    public bool StreetAddressRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.StreetAddress")]

    [JsonProperty("street_address")]
    public string StreetAddress { get; set; }


    [JsonProperty("street_address2_enabled")]
    public bool StreetAddress2Enabled { get; set; }

    [JsonProperty("street_address2_required")]
    public bool StreetAddress2Required { get; set; }
    [NopResourceDisplayName("Account.Fields.StreetAddress2")]

    [JsonProperty("street_address2")]
    public string StreetAddress2 { get; set; }


    [JsonProperty("zip_postal_code_enabled")]
    public bool ZipPostalCodeEnabled { get; set; }

    [JsonProperty("zip_postal_code_required")]
    public bool ZipPostalCodeRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.ZipPostalCode")]

    [JsonProperty("zip_postal_code")]
    public string ZipPostalCode { get; set; }


    [JsonProperty("city_enabled")]
    public bool CityEnabled { get; set; }

    [JsonProperty("city_required")]
    public bool CityRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.City")]

    [JsonProperty("city")]
    public string City { get; set; }


    [JsonProperty("county_enabled")]
    public bool CountyEnabled { get; set; }

    [JsonProperty("county_required")]
    public bool CountyRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.County")]

    [JsonProperty("county")]
    public string County { get; set; }


    [JsonProperty("country_enabled")]
    public bool CountryEnabled { get; set; }

    [JsonProperty("country_required")]
    public bool CountryRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.Country")]

    [JsonProperty("country_id")]
    public int CountryId { get; set; }

    [JsonProperty("available_countries")]
    public IList<SelectListItemDto> AvailableCountries { get; set; }
        

    [JsonProperty("state_province_enabled")]
    public bool StateProvinceEnabled { get; set; }

    [JsonProperty("state_province_required")]
    public bool StateProvinceRequired { get; set; }
    [NopResourceDisplayName("Account.Fields.StateProvince")]

    [JsonProperty("state_province_id")]
    public int StateProvinceId { get; set; }

    [JsonProperty("available_states")]
    public IList<SelectListItemDto> AvailableStates { get; set; }


    [JsonProperty("phone_enabled")]
    public bool PhoneEnabled { get; set; }

    [JsonProperty("phone_required")]
    public bool PhoneRequired { get; set; }
    [DataType(DataType.PhoneNumber)]
    [NopResourceDisplayName("Account.Fields.Phone")]

    [JsonProperty("phone")]
    public string Phone { get; set; }


    [JsonProperty("fax_enabled")]
    public bool FaxEnabled { get; set; }

    [JsonProperty("fax_required")]
    public bool FaxRequired { get; set; }
    [DataType(DataType.PhoneNumber)]
    [NopResourceDisplayName("Account.Fields.Fax")]

    [JsonProperty("fax")]
    public string Fax { get; set; }


    [JsonProperty("newsletter_enabled")]
    public bool NewsletterEnabled { get; set; }
    [NopResourceDisplayName("Account.Fields.Newsletter")]

    [JsonProperty("newsletter")]
    public bool Newsletter { get; set; }


    [JsonProperty("accept_privacy_policy_enabled")]
    public bool AcceptPrivacyPolicyEnabled { get; set; }

    [JsonProperty("accept_privacy_policy_popup")]
    public bool AcceptPrivacyPolicyPopup { get; set; }

    //time zone
    [NopResourceDisplayName("Account.Fields.TimeZone")]

    [JsonProperty("time_zone_id")]
    public string TimeZoneId { get; set; }

    [JsonProperty("allow_customers_to_set_time_zone")]
    public bool AllowCustomersToSetTimeZone { get; set; }

    [JsonProperty("available_time_zones")]
    public IList<SelectListItemDto> AvailableTimeZones { get; set; }

    //EU VAT
    [NopResourceDisplayName("Account.Fields.VatNumber")]

    [JsonProperty("vat_number")]
    public string VatNumber { get; set; }

    [JsonProperty("display_vat_number")]
    public bool DisplayVatNumber { get; set; }


    [JsonProperty("honeypot_enabled")]
    public bool HoneypotEnabled { get; set; }

    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }


    [JsonProperty("customer_attributes")]
    public IList<CustomerAttributeDto> CustomerAttributes { get; set; }


    [JsonProperty("gdpr_consents")]
    public IList<GdprConsentDto> GdprConsents { get; set; }
}
