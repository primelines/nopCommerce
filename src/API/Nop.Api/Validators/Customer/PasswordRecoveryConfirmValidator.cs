using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Customer;

namespace Nop.Api.Validators.Customer;

public partial class PasswordRecoveryConfirmValidator : BaseNopValidator<PasswordRecoveryConfirmDto>
{
    public PasswordRecoveryConfirmValidator(ILocalizationService localizationService, CustomerSettings customerSettings)
    {
        RuleFor(x => x.NewPassword).IsPassword(localizationService, customerSettings);
        RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.PasswordRecovery.ConfirmNewPassword.Required"));
        RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessageAwait(localizationService.GetResourceAsync("Account.PasswordRecovery.NewPassword.EnteredPasswordsDoNotMatch"));
    }
}