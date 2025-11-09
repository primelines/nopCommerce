using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "EmailRevalidation")]
public partial record EmailRevalidationDto : BaseNopDto
{

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("return_url")]
    public string ReturnUrl { get; set; }
}
