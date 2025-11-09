using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Common;

namespace Nop.Api.Validators.Common;

public partial class ContactUsValidator : BaseNopValidator<ContactUsDto>
{
    public ContactUsValidator(ILocalizationService localizationService, CommonSettings commonSettings)
    {
        RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ContactUs.Email.Required"));
        RuleFor(x => x.Email)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));
        RuleFor(x => x.FullName).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ContactUs.FullName.Required"));
        if (commonSettings.SubjectFieldOnContactUsForm)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ContactUs.Subject.Required"));
        }
        RuleFor(x => x.Enquiry).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("ContactUs.Enquiry.Required"));
    }
}