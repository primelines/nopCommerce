using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "ContactUs")]
public partial record ContactUsDto : BaseNopDto
{
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("ContactUs.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }

    [NopResourceDisplayName("ContactUs.Subject")]

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("subject_enabled")]
    public bool SubjectEnabled { get; set; }

    [NopResourceDisplayName("ContactUs.Enquiry")]

    [JsonProperty("enquiry")]
    public string Enquiry { get; set; }

    [NopResourceDisplayName("ContactUs.FullName")]

    [JsonProperty("full_name")]
    public string FullName { get; set; }


    [JsonProperty("successfully_sent")]
    public bool SuccessfullySent { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
