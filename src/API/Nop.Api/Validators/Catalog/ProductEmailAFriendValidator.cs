using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Catalog;

namespace Nop.Api.Validators.Catalog;

public partial class ProductEmailAFriendValidator : BaseNopValidator<ProductEmailAFriendDto>
{
    public ProductEmailAFriendValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.FriendEmail).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Products.EmailAFriend.FriendEmail.Required"));
        RuleFor(x => x.FriendEmail)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));

        RuleFor(x => x.YourEmailAddress).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Products.EmailAFriend.YourEmailAddress.Required"));
        RuleFor(x => x.YourEmailAddress)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));
    }
}