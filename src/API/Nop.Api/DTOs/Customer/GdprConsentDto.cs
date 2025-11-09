using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "GdprConsent")]
public partial record GdprConsentDto : BaseNopEntityDto
{

    [JsonProperty("message")]
    public string Message { get; set; }


    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }


    [JsonProperty("required_message")]
    public string RequiredMessage { get; set; }


    [JsonProperty("accepted")]
    public bool Accepted { get; set; }
}
