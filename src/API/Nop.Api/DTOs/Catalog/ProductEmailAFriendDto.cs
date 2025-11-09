using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductEmailAFriend")]
public partial record ProductEmailAFriendDto : BaseNopDto
{

    [JsonProperty("product_id")]
    public int ProductId { get; set; }


    [JsonProperty("product_name")]
    public string ProductName { get; set; }


    [JsonProperty("product_se_name")]
    public string ProductSeName { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Products.EmailAFriend.FriendEmail")]

    [JsonProperty("friend_email")]
    public string FriendEmail { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Products.EmailAFriend.YourEmailAddress")]

    [JsonProperty("your_email_address")]
    public string YourEmailAddress { get; set; }

    [NopResourceDisplayName("Products.EmailAFriend.PersonalMessage")]

    [JsonProperty("personal_message")]
    public string PersonalMessage { get; set; }


    [JsonProperty("successfully_sent")]
    public bool SuccessfullySent { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
