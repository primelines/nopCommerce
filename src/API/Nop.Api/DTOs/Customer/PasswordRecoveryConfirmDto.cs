using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "PasswordRecoveryConfirm")]
public partial record PasswordRecoveryConfirmDto : BaseNopDto
{
    [NoTrim]
    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.PasswordRecovery.NewPassword")]

    [JsonProperty("new_password")]
    public string NewPassword { get; set; }

    [NoTrim]
    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]

    [JsonProperty("confirm_new_password")]
    public string ConfirmNewPassword { get; set; }


    [JsonProperty("disable_password_changing")]
    public bool DisablePasswordChanging { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("return_url")]
    public string ReturnUrl { get; set; }
}
