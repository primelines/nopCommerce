using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "ContactVendor")]
public partial record ContactVendorDto : BaseNopDto
{

    [JsonProperty("vendor_id")]
    public int VendorId { get; set; }

    [JsonProperty("vendor_name")]
    public string VendorName { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("ContactVendor.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }

    [NopResourceDisplayName("ContactVendor.Subject")]

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("subject_enabled")]
    public bool SubjectEnabled { get; set; }

    [NopResourceDisplayName("ContactVendor.Enquiry")]

    [JsonProperty("enquiry")]
    public string Enquiry { get; set; }

    [NopResourceDisplayName("ContactVendor.FullName")]

    [JsonProperty("full_name")]
    public string FullName { get; set; }


    [JsonProperty("successfully_sent")]
    public bool SuccessfullySent { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
