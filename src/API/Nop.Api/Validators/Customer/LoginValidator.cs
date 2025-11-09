using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Customer;

namespace Nop.Api.Validators.Customer;

public partial class LoginValidator : BaseNopValidator<LoginDto>
{
    public LoginValidator(ILocalizationService localizationService, CustomerSettings customerSettings)
    {
        if (!customerSettings.UsernamesEnabled)
        {
            //login by email
            RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Login.Fields.Email.Required"));
            RuleFor(x => x.Email)
                .IsEmailAddress()
                .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));
        }
    }
}