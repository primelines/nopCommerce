using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "WishlistEmailAFriend")]
public partial record WishlistEmailAFriendDto : BaseNopDto
{
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Wishlist.EmailAFriend.FriendEmail")]

    [JsonProperty("friend_email")]
    public string FriendEmail { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Wishlist.EmailAFriend.YourEmailAddress")]

    [JsonProperty("your_email_address")]
    public string YourEmailAddress { get; set; }

    [NopResourceDisplayName("Wishlist.EmailAFriend.PersonalMessage")]

    [JsonProperty("personal_message")]
    public string PersonalMessage { get; set; }


    [JsonProperty("successfully_sent")]
    public bool SuccessfullySent { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }
}
