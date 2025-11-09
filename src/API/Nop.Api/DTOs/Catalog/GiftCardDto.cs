using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "GiftCard")]
public partial record GiftCardDto : BaseNopDto
{

    [JsonProperty("is_gift_card")]
    public bool IsGiftCard { get; set; }

    
    [JsonProperty("recipient_name")]
    public string RecipientName { get; set; }

    [DataType(DataType.EmailAddress)]

    [JsonProperty("recipient_email")]
    public string RecipientEmail { get; set; }


    [JsonProperty("sender_name")]
    public string SenderName { get; set; }

    [DataType(DataType.EmailAddress)]

    [JsonProperty("sender_email")]
    public string SenderEmail { get; set; }


    [JsonProperty("message")]
    public string Message { get; set; }


    [JsonProperty("gift_card_type")]
    public GiftCardType GiftCardType { get; set; }
}

