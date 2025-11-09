using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.PrivateMessages;

namespace Nop.Api.Validators.PrivateMessages;

public partial class SendPrivateMessageValidator : BaseNopValidator<SendPrivateMessageDto>
{
    public SendPrivateMessageValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Subject).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("PrivateMessages.SubjectCannotBeEmpty"));
        RuleFor(x => x.Message).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("PrivateMessages.MessageCannotBeEmpty"));
    }
}