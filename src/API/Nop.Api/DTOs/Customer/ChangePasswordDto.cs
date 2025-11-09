using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "ChangePassword")]
public partial record ChangePasswordDto : BaseNopDto
{
    [DataType(DataType.Password)]
    [NoTrim]
    [NopResourceDisplayName("Account.ChangePassword.Fields.OldPassword")]

    [JsonProperty("old_password")]
    public string OldPassword { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [NopResourceDisplayName("Account.ChangePassword.Fields.NewPassword")]

    [JsonProperty("new_password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [NopResourceDisplayName("Account.ChangePassword.Fields.ConfirmNewPassword")]

    [JsonProperty("confirm_new_password")]
    public string ConfirmNewPassword { get; set; }
}
