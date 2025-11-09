using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Blogs;

namespace Nop.Api.Validators.Blogs;

public partial class BlogPostValidator : BaseNopValidator<BlogPostDto>
{
    public BlogPostValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Blog.Comments.CommentText.Required")).When(x => x.AddNewComment != null);
    }
}