using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "PasswordRecovery")]
public partial record PasswordRecoveryDto : BaseNopDto
{
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.PasswordRecovery.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
