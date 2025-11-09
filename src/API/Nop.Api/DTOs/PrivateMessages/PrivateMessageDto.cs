using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.PrivateMessages;

[JsonObject(Title = "PrivateMessage")]
public partial record PrivateMessageDto : BaseNopEntityDto
{

    [JsonProperty("from_customer_id")]
    public int FromCustomerId { get; set; }

    [JsonProperty("customer_from_name")]
    public string CustomerFromName { get; set; }

    [JsonProperty("allow_viewing_from_profile")]
    public bool AllowViewingFromProfile { get; set; }


    [JsonProperty("to_customer_id")]
    public int ToCustomerId { get; set; }

    [JsonProperty("customer_to_name")]
    public string CustomerToName { get; set; }

    [JsonProperty("allow_viewing_to_profile")]
    public bool AllowViewingToProfile { get; set; }


    [JsonProperty("subject")]
    public string Subject { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }


    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }


    [JsonProperty("is_read")]
    public bool IsRead { get; set; }
}
