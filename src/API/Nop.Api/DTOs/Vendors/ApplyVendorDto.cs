using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Vendors;

[JsonObject(Title = "ApplyVendor")]
public partial record ApplyVendorDto : BaseNopDto
{
    public ApplyVendorDto()
    {
        VendorAttributes = new List<VendorAttributeDto>();
    }

    [NopResourceDisplayName("Vendors.ApplyAccount.Name")]

    [JsonProperty("name")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Vendors.ApplyAccount.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Vendors.ApplyAccount.Description")]

    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("vendor_attributes")]
    public IList<VendorAttributeDto> VendorAttributes { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }


    [JsonProperty("terms_of_service_enabled")]
    public bool TermsOfServiceEnabled { get; set; }

    [JsonProperty("terms_of_service_popup")]
    public bool TermsOfServicePopup { get; set; }


    [JsonProperty("disable_form_input")]
    public bool DisableFormInput { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }
}
