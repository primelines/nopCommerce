using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Newsletter;

[JsonObject(Title = "NewsletterBox")]
public partial record NewsletterBoxDto : BaseNopDto
{

    [JsonProperty("allow_to_unsubscribe")]
    public bool AllowToUnsubscribe { get; set; }

    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
    [DataType(DataType.EmailAddress)]

    [JsonProperty("newsletter_email")]
    public string NewsletterEmail { get; set; }

    [JsonProperty("is_re_captcha_v3")]
    public bool IsReCaptchaV3 { get; set; }

    [JsonProperty("re_captcha_public_key")]
    public string ReCaptchaPublicKey { get; set; }
}
