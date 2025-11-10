using System.Globalization;
using System.Text.Encodings.Web;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.DTOs.Catalog;
using Nop.Api.DTOs.JsonLD;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the JSON-LD model factory implementation
/// </summary>
public partial class JsonLdDtoFactory : IJsonLdDtoFactory
{
    #region Fields

    protected readonly IEventPublisher _eventPublisher;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public JsonLdDtoFactory(IEventPublisher eventPublisher,
        INopUrlHelper nopUrlHelper,
        IWebHelper webHelper)
    {
        _eventPublisher = eventPublisher;
        _nopUrlHelper = nopUrlHelper;
        _webHelper = webHelper;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Prepare JSON-LD category breadcrumb model
    /// </summary>
    /// <param name="categoryModels">List of category models</param>
    /// <returns>A task that represents the asynchronous operation
    /// The task result contains JSON-LD category breadcrumb model
    /// </returns>
    protected virtual async Task<JsonLdBreadcrumbListDto> PrepareJsonLdBreadcrumbListAsync(IList<CategorySimpleDto> categoryModels)
    {
        var breadcrumbList = new JsonLdBreadcrumbListDto();
        var position = 1;

        foreach (var cat in categoryModels)
        {
            var breadcrumbListItem = new JsonLdBreadcrumbListItemDto
            {
                Position = position,
                Item = new JsonLdBreadcrumbItemDto
                {
                    Id = await _nopUrlHelper.RouteGenericUrlAsync<Category>(new { SeName = cat.SeName }, _webHelper.GetCurrentRequestProtocol()),
                    Name = cat.Name
                }
            };
            breadcrumbList.ItemListElement.Add(breadcrumbListItem);
            position++;
        }

        return breadcrumbList;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare JSON-LD category breadcrumb model
    /// </summary>
    /// <param name="categoryModels">List of category models</param>
    /// <returns>A task that represents the asynchronous operation
    /// The task result contains JSON-LD category breadcrumb model
    /// </returns>
    public virtual async Task<JsonLdBreadcrumbListDto> PrepareJsonLdCategoryBreadcrumbAsync(IList<CategorySimpleDto> categoryModels)
    {
        var breadcrumbList = await PrepareJsonLdBreadcrumbListAsync(categoryModels);

        await _eventPublisher.PublishAsync(new JsonLdCreatedEvent<JsonLdBreadcrumbListDto>(breadcrumbList));

        return breadcrumbList;
    }

    /// <summary>
    /// Prepare JSON-LD product breadcrumb model
    /// </summary>
    /// <param name="breadcrumbModel">Product breadcrumb model</param>
    /// <returns>A task that represents the asynchronous operation
    /// The task result contains JSON-LD product breadcrumb model
    /// </returns>
    public virtual async Task<JsonLdBreadcrumbListDto> PrepareJsonLdProductBreadcrumbAsync(ProductBreadcrumbDto breadcrumbModel)
    {
        var breadcrumbList = await PrepareJsonLdBreadcrumbListAsync(breadcrumbModel.CategoryBreadcrumb);

        breadcrumbList.ItemListElement.Add(new JsonLdBreadcrumbListItemDto
        {
            Position = breadcrumbList.ItemListElement.Count + 1,
            Item = new JsonLdBreadcrumbItemDto
            {
                Id = await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = breadcrumbModel.ProductSeName }, _webHelper.GetCurrentRequestProtocol()),
                Name = breadcrumbModel.ProductName,
            }
        });

        await _eventPublisher.PublishAsync(new JsonLdCreatedEvent<JsonLdBreadcrumbListDto>(breadcrumbList));

        return breadcrumbList;
    }

    /// <summary>
    /// Prepare JSON-LD product model
    /// </summary>
    /// <param name="model">Product details model</param>
    /// <param name="productUrl">Product URL</param>
    /// <returns>A task that represents the asynchronous operation
    /// The task result contains JSON-LD product model
    /// </returns>
    public virtual async Task<JsonLdProductDto> PrepareJsonLdProductAsync(ProductDetailsDto model, string productUrl = null)
    {
        productUrl ??= await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = model.SeName }, _webHelper.GetCurrentRequestProtocol());

        var productPrice = model.AssociatedProducts.Any()
            ? model.AssociatedProducts.Min(associatedProduct => associatedProduct.ProductPrice.PriceValue)
            : model.ProductPrice.PriceValue;

        var product = new JsonLdProductDto
        {
            Name = model.Name,
            Sku = model.Sku,
            Gtin = model.Gtin,
            Mpn = model.ManufacturerPartNumber,
            Description = model.ShortDescription,
            Image = model.DefaultPicture.ImageUrl,
            Offer = new JsonLdOfferDto
            {
                Url = productUrl.ToLowerInvariant(),
                Price = productPrice.ToString("0.00", CultureInfo.InvariantCulture),
                PriceCurrency = model.ProductPrice.CurrencyCode,
                PriceValidUntil = model.AvailableEndDate,
                Availability = @"https://schema.org/" + (model.InStock ? "InStock" : "OutOfStock")
            },
            Brand = model.ProductManufacturers?.Select(manufacturer => new JsonLdBrandDto { Name = manufacturer.Name }).ToList()
        };

        if (model.ProductReviewOverview.TotalReviews > 0)
        {
            var ratingPercent = model.ProductReviewOverview.RatingSum * 100 / model.ProductReviewOverview.TotalReviews / 5;

            var ratingValue = ratingPercent / (decimal)20;

            product.AggregateRating = new JsonLdAggregateRatingDto
            {
                RatingValue = ratingValue.ToString("0.0", CultureInfo.InvariantCulture),
                ReviewCount = model.ProductReviewOverview.TotalReviews
            };

            product.Review = model.ProductReviews.Items?.Select(review => new JsonLdReviewDto
            {
                Name = JavaScriptEncoder.Default.Encode(review.Title),
                ReviewBody = JavaScriptEncoder.Default.Encode(review.ReviewText),
                ReviewRating = new JsonLdRatingDto
                {
                    RatingValue = review.Rating
                },
                Author = new JsonLdPersonDto { Name = JavaScriptEncoder.Default.Encode(review.CustomerName) },
                DatePublished = review.WrittenOnStr
            }).ToList();
        }

        foreach (var associatedProduct in model.AssociatedProducts)
        {
            var parentUrl = !associatedProduct.VisibleIndividually ? productUrl : null;
            product.HasVariant.Add(await PrepareJsonLdProductAsync(associatedProduct, parentUrl));
        }

        await _eventPublisher.PublishAsync(new JsonLdCreatedEvent<JsonLdProductDto>(product));

        return product;
    }

    #endregion
}