using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "MultiFactorAuthentication")]
public partial record MultiFactorAuthenticationDto : BaseNopDto
{
    public MultiFactorAuthenticationDto()
    {
        Providers = new List<MultiFactorAuthenticationProviderDto>();
    }

    [NopResourceDisplayName("Account.MultiFactorAuthentication.Fields.IsEnabled")]

    [JsonProperty("is_enabled")]
    public bool IsEnabled { get; set; }


    [JsonProperty("providers")]
    public List<MultiFactorAuthenticationProviderDto> Providers { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }

}
