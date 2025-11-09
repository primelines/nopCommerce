using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Customer;

namespace Nop.Api.Validators.Customer;

public partial class PasswordRecoveryValidator : BaseNopValidator<PasswordRecoveryDto>
{
    public PasswordRecoveryValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.PasswordRecovery.Email.Required"));
        RuleFor(x => x.Email)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));
    }
}