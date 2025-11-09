using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Customer;

namespace Nop.Api.Validators.Customer;

public partial class GiftCardValidator : BaseNopValidator<CheckGiftCardBalanceDto>
{
    public GiftCardValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.GiftCardCode).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CheckGiftCardBalance.GiftCardCouponCode.Empty"));
    }
}