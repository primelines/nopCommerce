using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.PrivateMessages;

[JsonObject(Title = "SendPrivateMessage")]
public partial record SendPrivateMessageDto : BaseNopEntityDto
{

    [JsonProperty("to_customer_id")]
    public int ToCustomerId { get; set; }

    [JsonProperty("customer_to_name")]
    public string CustomerToName { get; set; }

    [JsonProperty("allow_viewing_to_profile")]
    public bool AllowViewingToProfile { get; set; }


    [JsonProperty("reply_to_message_id")]
    public int ReplyToMessageId { get; set; }


    [JsonProperty("subject")]
    public string Subject { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }
}
