using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.RFQ.Models.Admin;

public record GiftCardModel : BaseNopModel
{
    public bool IsGiftCard { get; set; }

    [NopResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
    public string RecipientName { get; set; }
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
    public string RecipientEmail { get; set; }
    [NopResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
    public string SenderName { get; set; }
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
    public string SenderEmail { get; set; }
    [NopResourceDisplayName("Admin.GiftCards.Fields.Message")]
    public string Message { get; set; }

    public GiftCardType GiftCardType { get; set; }
}