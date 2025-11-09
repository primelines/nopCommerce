using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Customers;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "Login")]
public partial record LoginDto : BaseNopDto
{

    [JsonProperty("checkout_as_guest")]
    public bool CheckoutAsGuest { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.Login.Fields.Email")]

    [JsonProperty("email")]
    public string Email { get; set; }


    [JsonProperty("usernames_enabled")]
    public bool UsernamesEnabled { get; set; }


    [JsonProperty("registration_type")]
    public UserRegistrationType RegistrationType { get; set; }

    [NopResourceDisplayName("Account.Login.Fields.Username")]

    [JsonProperty("username")]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [NopResourceDisplayName("Account.Login.Fields.Password")]

    [JsonProperty("password")]
    public string Password { get; set; }

    [NopResourceDisplayName("Account.Login.Fields.RememberMe")]

    [JsonProperty("remember_me")]
    public bool RememberMe { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
