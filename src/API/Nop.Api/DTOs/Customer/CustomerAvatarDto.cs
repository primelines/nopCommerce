using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerAvatar")]
public partial record CustomerAvatarDto : BaseNopDto
{

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }
}
