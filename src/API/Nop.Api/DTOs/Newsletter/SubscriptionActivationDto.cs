using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Newsletter;

[JsonObject(Title = "SubscriptionActivation")]
public partial record SubscriptionActivationDto : BaseNopDto
{

    [JsonProperty("result")]
    public string Result { get; set; }
}
