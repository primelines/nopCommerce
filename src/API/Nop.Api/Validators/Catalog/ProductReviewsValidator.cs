using FluentValidation;
using Nop.Services.Localization;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Catalog;

namespace Nop.Api.Validators.Catalog;

public partial class ProductReviewsValidator : BaseNopValidator<ProductReviewsDto>
{
    public ProductReviewsValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.AddProductReview.Title).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Reviews.Fields.Title.Required")).When(x => x.AddProductReview != null);
        RuleFor(x => x.AddProductReview.Title).Length(1, 200).WithMessageAwait(localizationService.GetResourceAsync("Reviews.Fields.Title.MaxLengthValidation"), 200).When(x => x.AddProductReview != null && !string.IsNullOrEmpty(x.AddProductReview.Title));
        RuleFor(x => x.AddProductReview.ReviewText).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Reviews.Fields.ReviewText.Required")).When(x => x.AddProductReview != null);
    }
}