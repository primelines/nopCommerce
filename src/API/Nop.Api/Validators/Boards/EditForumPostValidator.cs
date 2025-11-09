using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Boards;

namespace Nop.Api.Validators.Boards;

public partial class EditForumPostValidator : BaseNopValidator<EditForumPostDto>
{
    public EditForumPostValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Text).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Forum.TextCannotBeEmpty"));
    }
}