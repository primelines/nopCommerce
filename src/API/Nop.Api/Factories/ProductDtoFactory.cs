using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Api.Infrastructure.Cache;
using Nop.Api.DTOs.Catalog;
using Nop.Api.DTOs.Common;
using Nop.Api.DTOs.Media;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Videos;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the product model factory
/// </summary>
public partial class ProductDtoFactory : IProductDtoFactory
{
    private readonly IProductReviewService _productReviewService;
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CatalogSettings _catalogSettings;
    protected readonly CustomerSettings _customerSettings;
    protected readonly ICategoryService _categoryService;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerService _customerService;
    protected readonly IDateRangeService _dateRangeService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IDownloadService _downloadService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IJsonLdDtoFactory _jsonLdModelFactory;
    protected readonly ILocalizationService _localizationService;
    protected readonly IManufacturerService _manufacturerService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IPriceCalculationService _priceCalculationService;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly IProductService _productService;
    protected readonly IProductTagService _productTagService;
    protected readonly IProductTemplateService _productTemplateService;
    protected readonly IReviewTypeService _reviewTypeService;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly ISpecificationAttributeService _specificationAttributeService;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreService _storeService;
    protected readonly IShoppingCartDtoFactory _shoppingCartModelFactory;
    protected readonly ITaxService _taxService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVendorService _vendorService;
    protected readonly IVideoService _videoService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly MediaSettings _mediaSettings;
    protected readonly OrderSettings _orderSettings;
    protected readonly SeoSettings _seoSettings;
    protected readonly ShippingSettings _shippingSettings;
    protected readonly VendorSettings _vendorSettings;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public ProductDtoFactory(IProductReviewService productReviewService, CaptchaSettings captchaSettings,
        CatalogSettings catalogSettings,
        CustomerSettings customerSettings,
        ICategoryService categoryService,
        ICurrencyService currencyService,
        ICustomerService customerService,
        IDateRangeService dateRangeService,
        IDateTimeHelper dateTimeHelper,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IJsonLdDtoFactory jsonLdModelFactory,
        ILocalizationService localizationService,
        IManufacturerService manufacturerService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceCalculationService priceCalculationService,
        IPriceFormatter priceFormatter,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IProductService productService,
        IProductTagService productTagService,
        IProductTemplateService productTemplateService,
        IReviewTypeService reviewTypeService,
        IShoppingCartService shoppingCartService,
        ISpecificationAttributeService specificationAttributeService,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IStoreService storeService,
        IShoppingCartDtoFactory shoppingCartModelFactory,
        ITaxService taxService,
        IUrlRecordService urlRecordService,
        IVendorService vendorService,
        IVideoService videoService,
        IWebHelper webHelper,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        OrderSettings orderSettings,
        SeoSettings seoSettings,
        ShippingSettings shippingSettings,
        VendorSettings vendorSettings)
    {
        _productReviewService = productReviewService;
        _captchaSettings = captchaSettings;
        _catalogSettings = catalogSettings;
        _customerSettings = customerSettings;
        _categoryService = categoryService;
        _currencyService = currencyService;
        _customerService = customerService;
        _dateRangeService = dateRangeService;
        _dateTimeHelper = dateTimeHelper;
        _downloadService = downloadService;
        _genericAttributeService = genericAttributeService;
        _jsonLdModelFactory = jsonLdModelFactory;
        _localizationService = localizationService;
        _manufacturerService = manufacturerService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _priceCalculationService = priceCalculationService;
        _priceFormatter = priceFormatter;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _productService = productService;
        _productTagService = productTagService;
        _productTemplateService = productTemplateService;
        _reviewTypeService = reviewTypeService;
        _shoppingCartService = shoppingCartService;
        _specificationAttributeService = specificationAttributeService;
        _staticCacheManager = staticCacheManager;
        _storeContext = storeContext;
        _storeService = storeService;
        _shoppingCartModelFactory = shoppingCartModelFactory;
        _taxService = taxService;
        _urlRecordService = urlRecordService;
        _vendorService = vendorService;
        _webHelper = webHelper;
        _workContext = workContext;
        _mediaSettings = mediaSettings;
        _orderSettings = orderSettings;
        _seoSettings = seoSettings;
        _shippingSettings = shippingSettings;
        _vendorSettings = vendorSettings;
        _videoService = videoService;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Prepare the product specification models
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="group">Specification attribute group</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of product specification model
    /// </returns>
    protected virtual async Task<IList<ProductSpecificationAttributeDto>> PrepareProductSpecificationAttributeDtoAsync(Product product, SpecificationAttributeGroup group)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productSpecificationAttributes = await _specificationAttributeService.GetProductSpecificationAttributesAsync(
            product.Id, specificationAttributeGroupId: group?.Id, showOnProductPage: true);

        var result = new List<ProductSpecificationAttributeDto>();

        foreach (var psa in productSpecificationAttributes)
        {
            var option = await _specificationAttributeService.GetSpecificationAttributeOptionByIdAsync(psa.SpecificationAttributeOptionId);

            var model = result.FirstOrDefault(model => model.Id == option.SpecificationAttributeId);
            if (model == null)
            {
                var attribute = await _specificationAttributeService.GetSpecificationAttributeByIdAsync(option.SpecificationAttributeId);
                model = new ProductSpecificationAttributeDto
                {
                    Id = attribute.Id,
                    Name = await _localizationService.GetLocalizedAsync(attribute, x => x.Name)
                };
                result.Add(model);
            }

            var value = new ProductSpecificationAttributeValueDto
            {
                AttributeTypeId = psa.AttributeTypeId,
                ColorSquaresRgb = option.ColorSquaresRgb,
                ValueRaw = psa.AttributeType switch
                {
                    SpecificationAttributeType.Option => WebUtility.HtmlEncode(await _localizationService.GetLocalizedAsync(option, x => x.Name)),
                    SpecificationAttributeType.CustomText => WebUtility.HtmlEncode(await _localizationService.GetLocalizedAsync(psa, x => x.CustomValue)),
                    SpecificationAttributeType.CustomHtmlText => await _localizationService.GetLocalizedAsync(psa, x => x.CustomValue),
                    SpecificationAttributeType.Hyperlink => psa.CustomValue,
                    _ => null
                }
            };

            model.Values.Add(value);
        }

        return result;
    }

    /// <summary>
    /// Prepare the product review overview model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product review overview model
    /// </returns>
    protected virtual async Task<ProductReviewOverviewDto> PrepareProductReviewOverviewModelAsync(Product product)
    {
        ProductReviewOverviewDto productReview;
        var currentStore = await _storeContext.GetCurrentStoreAsync();

        if (_catalogSettings.ShowProductReviewsPerStore)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductReviewsModelKey, product, currentStore);

            productReview = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var productReviews = await _productReviewService.GetAllProductReviewsAsync(productId: product.Id, approved: true, storeId: currentStore.Id);

                return new ProductReviewOverviewDto
                {
                    RatingSum = productReviews.Sum(pr => pr.Rating),
                    TotalReviews = productReviews.Count
                };
            });
        }
        else
        {
            productReview = new ProductReviewOverviewDto
            {
                RatingSum = product.ApprovedRatingSum,
                TotalReviews = product.ApprovedTotalReviews
            };
        }

        if (productReview != null)
        {
            productReview.ProductId = product.Id;
            productReview.CanCurrentCustomerLeaveReview = _catalogSettings.AllowAnonymousUsersToReviewProduct || !await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync());
            productReview.CanAddNewReview = await _productReviewService.CanAddReviewAsync(product.Id, _catalogSettings.ShowProductReviewsPerStore ? currentStore.Id : 0);
        }

        return productReview;
    }

    /// <summary>
    /// Prepare the product overview price model
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="forceRedirectionAfterAddingToCart">Whether to force redirection after adding to cart</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product overview price model
    /// </returns>
    protected virtual async Task<ProductPriceOverviewDto> PrepareProductOverviewPriceModelAsync(Product product, bool forceRedirectionAfterAddingToCart = false)
    {
        ArgumentNullException.ThrowIfNull(product);

        var priceModel = new ProductPriceOverviewDto
        {
            ForceRedirectionAfterAddingToCart = forceRedirectionAfterAddingToCart
        };

        await PrepareSimpleProductOverviewPriceModelAsync(product, priceModel);
        return priceModel;
    }

    /// <summary>
    /// Prepare the simple product overview price model
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="priceModel">Price model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task PrepareSimpleProductOverviewPriceModelAsync(Product product, ProductPriceOverviewDto priceModel)
    {
        //add to cart button
        priceModel.DisableBuyButton = product.DisableBuyButton ||
                                      !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART) ||
                                      !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES);

        //add to wishlist button
        priceModel.DisableWishlistButton = product.DisableWishlistButton ||
                                           !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST) ||
                                           !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES);
        //compare products
        priceModel.DisableAddToCompareListButton = !_catalogSettings.CompareProductsEnabled;

        //pre-order
        if (product.AvailableForPreOrder)
        {
            priceModel.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
                                              product.PreOrderAvailabilityStartDateTimeUtc.Value >=
                                              DateTime.UtcNow;
            priceModel.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;
        }

        //prices
        if (await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var customer = await _workContext.GetCurrentCustomerAsync();

            //prices
            var (minPossiblePriceWithoutDiscount, minPossiblePriceWithDiscount) = (decimal.Zero, decimal.Zero);
            var hasMultiplePrices = false;
            if (_catalogSettings.DisplayFromPrices)
            {
                var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);
                var cacheKey = _staticCacheManager
                    .PrepareKeyForDefaultCache(NopCatalogDefaults.ProductMultiplePriceCacheKey, product, customerRoleIds, store);
                if (!_catalogSettings.CacheProductPrices)
                    cacheKey.CacheTime = 0;

                var cachedPrice = await _staticCacheManager.GetAsync(cacheKey, async () =>
                {
                    var prices = new List<(decimal PriceWithoutDiscount, decimal PriceWithDiscount)>();

                    // price when there are no required attributes
                    var attributesMappings = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(product.Id);
                    if (!attributesMappings.Any(am => !am.IsNonCombinable() && am.IsRequired))
                    {
                        (var priceWithoutDiscount, var priceWithDiscount, _, _) = await _priceCalculationService
                            .GetFinalPriceAsync(product, customer, store);
                        prices.Add((priceWithoutDiscount, priceWithDiscount));
                    }

                    var allAttributesXml = await _productAttributeParser.GenerateAllCombinationsAsync(product, true);
                    foreach (var attributesXml in allAttributesXml)
                    {
                        var warnings = new List<string>();
                        warnings.AddRange(await _shoppingCartService.GetShoppingCartItemAttributeWarningsAsync(customer,
                            ShoppingCartType.ShoppingCart, product, 1, attributesXml, true, true, true));
                        if (warnings.Any())
                            continue;

                        //get price with additional charge
                        var combination = await _productAttributeParser.FindProductAttributeCombinationAsync(product, attributesXml);
                        if (combination?.OverriddenPrice.HasValue ?? false)
                        {
                            (var priceWithoutDiscount, var priceWithDiscount, _, _) = await _priceCalculationService
                                .GetFinalPriceAsync(product, customer, store, combination.OverriddenPrice.Value, decimal.Zero, true, 1);
                            prices.Add((priceWithoutDiscount, priceWithDiscount));
                        }
                        else
                        {
                            var additionalCharge = decimal.Zero;
                            var attributeValues = await _productAttributeParser.ParseProductAttributeValuesAsync(attributesXml);
                            foreach (var attributeValue in attributeValues)
                            {
                                additionalCharge += await _priceCalculationService.
                                    GetProductAttributeValuePriceAdjustmentAsync(product, attributeValue, customer, store);
                            }
                            if (additionalCharge != decimal.Zero)
                            {
                                (var priceWithoutDiscount, var priceWithDiscount, _, _) = await _priceCalculationService
                                    .GetFinalPriceAsync(product, customer, store, additionalCharge);
                                prices.Add((priceWithoutDiscount, priceWithDiscount));
                            }
                        }
                    }

                    if (prices.Distinct().Count() > 1)
                    {
                        (minPossiblePriceWithoutDiscount, minPossiblePriceWithDiscount) = prices.OrderBy(p => p.PriceWithDiscount).First();
                        return new
                        {
                            PriceWithoutDiscount = minPossiblePriceWithoutDiscount,
                            PriceWithDiscount = minPossiblePriceWithDiscount
                        };
                    }

                    // show default price when required attributes available but no values added
                    (minPossiblePriceWithoutDiscount, minPossiblePriceWithDiscount, _, _) = await _priceCalculationService.GetFinalPriceAsync(product, customer, store);

                    //don't cache (return null) if there are no multiple prices
                    return null;
                });

                if (cachedPrice is not null)
                {
                    hasMultiplePrices = true;
                    (minPossiblePriceWithoutDiscount, minPossiblePriceWithDiscount) = (cachedPrice.PriceWithoutDiscount, cachedPrice.PriceWithDiscount);
                }
            }
            else
                (minPossiblePriceWithoutDiscount, minPossiblePriceWithDiscount, _, _) = await _priceCalculationService.GetFinalPriceAsync(product, customer, store);


            //do we have tier prices configured?
            var tierPrices = await _productService.GetTierPricesAsync(product, customer, store);


            if (tierPrices.Any())
            {
                var (tierPriceMinPossiblePriceWithoutDiscount, tierPriceMinPossiblePriceWithDiscount, _, _) = await _priceCalculationService.GetFinalPriceAsync(product, customer, store, quantity: int.MaxValue);

                //calculate price for the maximum quantity if we have tier prices, and choose minimal
                minPossiblePriceWithoutDiscount = Math.Min(minPossiblePriceWithoutDiscount, tierPriceMinPossiblePriceWithoutDiscount);
                minPossiblePriceWithDiscount = Math.Min(minPossiblePriceWithDiscount, tierPriceMinPossiblePriceWithDiscount);
            }

            var (oldPriceBase, _) = await _taxService.GetProductPriceAsync(product, product.OldPrice);
            var (finalPriceWithoutDiscountBase, _) = await _taxService.GetProductPriceAsync(product, minPossiblePriceWithoutDiscount);
            var (finalPriceWithDiscountBase, _) = await _taxService.GetProductPriceAsync(product, minPossiblePriceWithDiscount);
            var currentCurrency = await _workContext.GetWorkingCurrencyAsync();
            var oldPrice = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(oldPriceBase, currentCurrency);
            var finalPriceWithoutDiscount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(finalPriceWithoutDiscountBase, currentCurrency);
            var finalPriceWithDiscount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(finalPriceWithDiscountBase, currentCurrency);

            var strikeThroughPrice = decimal.Zero;

            if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
                strikeThroughPrice = oldPrice;

            if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
                strikeThroughPrice = finalPriceWithoutDiscount;

            if (strikeThroughPrice > decimal.Zero)
            {
                priceModel.OldPrice = await _priceFormatter.FormatPriceAsync(strikeThroughPrice);
                priceModel.OldPriceValue = strikeThroughPrice;
            }
            else
            {
                priceModel.OldPrice = null;
                priceModel.OldPriceValue = null;
            }



            //When there is just one tier price (with  qty 1), there are no actual savings in the list.
            var hasTierPrices = tierPrices.Any() && !(tierPrices.Count == 1 && tierPrices[0].Quantity <= 1);

            var price = await _priceFormatter.FormatPriceAsync(finalPriceWithDiscount);
            priceModel.Price = hasTierPrices || hasMultiplePrices
                ? string.Format(await _localizationService.GetResourceAsync("Products.PriceRangeFrom"), price)
                : price;
            priceModel.PriceValue = finalPriceWithDiscount;

            //property for German market
            //we display tax/shipping info only with "shipping enabled" for this product
            //we also ensure this it's not free shipping
            priceModel.DisplayTaxShippingInfo = _catalogSettings.DisplayTaxShippingInfoProductBoxes && product.IsShipEnabled && !product.IsFreeShipping;            
        }
        else
        {
            //hide prices
            priceModel.OldPrice = null;
            priceModel.OldPriceValue = null;
            priceModel.Price = null;
            priceModel.PriceValue = null;
        }
    }

    /// <summary>
    /// Prepare the product overview picture model
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="productThumbPictureSize">Product thumb picture size (longest side); pass null to use the default value of media settings</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains picture models
    /// </returns>
    protected virtual async Task<IList<PictureDto>> PrepareProductOverviewPicturesModelAsync(Product product, int? productThumbPictureSize = null)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productName = await _localizationService.GetLocalizedAsync(product, x => x.Name);
        //If a size has been set in the view, we use it in priority
        var pictureSize = productThumbPictureSize ?? _mediaSettings.ProductThumbPictureSize;

        //prepare picture model
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductOverviewPicturesModelKey,
            product, pictureSize, true, _catalogSettings.DisplayAllPicturesOnCatalogPages, await _workContext.GetWorkingLanguageAsync(),
            _webHelper.IsCurrentConnectionSecured(), await _storeContext.GetCurrentStoreAsync());

        var cachedPictures = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            async Task<PictureDto> preparePictureDtoAsync(Picture picture)
            {
                //we have to keep the url generation order "full size -> preview" because picture can be updated twice
                //this section of code requires detailed analysis in the future
                (var fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                (var imageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture, pictureSize);

                return new PictureDto
                {
                    ImageUrl = imageUrl,
                    FullSizeImageUrl = fullSizeImageUrl,
                    //"title" attribute
                    Title = (picture != null && !string.IsNullOrEmpty(picture.TitleAttribute))
                        ? picture.TitleAttribute
                        : string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageLinkTitleFormat"),
                            productName),
                    //"alt" attribute
                    AlternateText = (picture != null && !string.IsNullOrEmpty(picture.AltAttribute))
                        ? picture.AltAttribute
                        : string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageAlternateTextFormat"),
                            productName)
                };
            }

            //all pictures
            var pictures = (await _pictureService
                    .GetPicturesByProductIdAsync(product.Id, _catalogSettings.DisplayAllPicturesOnCatalogPages ? 0 : 1))
                .DefaultIfEmpty(null);
            var pictureModels = await pictures
                .SelectAwait(async picture => await preparePictureDtoAsync(picture))
                .ToListAsync();
            return pictureModels;
        });

        return cachedPictures;
    }

    /// <summary>
    /// Prepare the product breadcrumb model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product breadcrumb model
    /// </returns>
    protected virtual async Task<ProductBreadcrumbDto> PrepareProductBreadcrumbModelAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var breadcrumbModel = new ProductBreadcrumbDto
        {
            Enabled = _catalogSettings.CategoryBreadcrumbEnabled,
            ProductId = product.Id,
            ProductName = await _localizationService.GetLocalizedAsync(product, x => x.Name),
            ProductSeName = await _urlRecordService.GetSeNameAsync(product)
        };
        var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id);
        if (!productCategories.Any())
            return breadcrumbModel;

        var category = await _categoryService.GetCategoryByIdAsync(productCategories[0].CategoryId);
        if (category == null)
            return breadcrumbModel;

        foreach (var catBr in await _categoryService.GetCategoryBreadCrumbAsync(category))
        {
            breadcrumbModel.CategoryBreadcrumb.Add(new CategorySimpleDto
            {
                Id = catBr.Id,
                Name = await _localizationService.GetLocalizedAsync(catBr, x => x.Name),
                SeName = await _urlRecordService.GetSeNameAsync(catBr),
                //IncludeInTopMenu = catBr.IncludeInTopMenu
            });
        }

        if (_seoSettings.MicrodataEnabled)
        {
            var jsonLdModel = await _jsonLdModelFactory.PrepareJsonLdProductBreadcrumbAsync(breadcrumbModel);
            breadcrumbModel.JsonLd = JsonConvert
                .SerializeObject(jsonLdModel, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        return breadcrumbModel;
    }

    /// <summary>
    /// Prepare the product tag models
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of product tag model
    /// </returns>
    protected virtual async Task<IList<ProductTagDto>> PrepareProductTagDtosAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var store = await _storeContext.GetCurrentStoreAsync();
        var productsTags = await _productTagService.GetAllProductTagsByProductIdAsync(product.Id);

        var model = await productsTags
            //filter by store
            .WhereAwait(async x => await _productTagService.GetProductCountByProductTagIdAsync(x.Id, store.Id) > 0)
            .SelectAwait(async x => new ProductTagDto
            {
                Id = x.Id,
                Name = await _localizationService.GetLocalizedAsync(x, y => y.Name),
                SeName = await _urlRecordService.GetSeNameAsync(x),
                ProductCount = await _productTagService.GetProductCountByProductTagIdAsync(x.Id, store.Id)
            }).ToListAsync();

        return model;
    }

    /// <summary>
    /// Prepare the product price model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product price model
    /// </returns>
    protected virtual async Task<ProductPriceDto> PrepareProductPriceModelAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = new ProductPriceDto
        {
            ProductId = product.Id
        };

        if (await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
        {
            model.HidePrices = false;

            var customer = await _workContext.GetCurrentCustomerAsync();
            var store = await _storeContext.GetCurrentStoreAsync();
            var currentCurrency = await _workContext.GetWorkingCurrencyAsync();

            var (oldPriceBase, _) = await _taxService.GetProductPriceAsync(product, product.OldPrice);

            var (finalPriceWithoutDiscountBase, _) = await _taxService.GetProductPriceAsync(product, (await _priceCalculationService.GetFinalPriceAsync(product, customer, store, includeDiscounts: false)).finalPrice);
            var (finalPriceWithDiscountBase, _) = await _taxService.GetProductPriceAsync(product, (await _priceCalculationService.GetFinalPriceAsync(product, customer, store)).finalPrice);

            var oldPrice = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(oldPriceBase, currentCurrency);
            var finalPriceWithoutDiscount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(finalPriceWithoutDiscountBase, currentCurrency);
            var finalPriceWithDiscount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(finalPriceWithDiscountBase, currentCurrency);

            if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
            {
                model.OldPrice = await _priceFormatter.FormatPriceAsync(oldPrice);
                model.OldPriceValue = oldPrice;
            }

            model.Price = await _priceFormatter.FormatPriceAsync(finalPriceWithoutDiscount);

            if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
            {
                model.PriceWithDiscount = await _priceFormatter.FormatPriceAsync(finalPriceWithDiscount);
                model.PriceWithDiscountValue = finalPriceWithDiscount;
            }

            model.PriceValue = finalPriceWithDiscount;

            //property for German market
            //we display tax/shipping info only with "shipping enabled" for this product
            //we also ensure this it's not free shipping
            model.DisplayTaxShippingInfo = _catalogSettings.DisplayTaxShippingInfoProductDetailsPage
                                            && product.IsShipEnabled &&
                                            !product.IsFreeShipping;
            //currency code
            model.CurrencyCode = currentCurrency.CurrencyCode;

        }
        else
        {
            model.HidePrices = true;
            model.OldPrice = null;
            model.OldPriceValue = null;
            model.Price = null;
        }

        return model;
    }

    /// <summary>
    /// Prepare the product add to cart model
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="updatecartitem">Updated shopping cart item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product add to cart model
    /// </returns>
    protected virtual async Task<AddToCartDto> PrepareProductAddToCartModelAsync(Product product, ShoppingCartItem updatecartitem)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = new AddToCartDto
        {
            ProductId = product.Id
        };

        if (updatecartitem != null)
        {
            model.UpdatedShoppingCartItemId = updatecartitem.Id;
            model.UpdateShoppingCartItemType = updatecartitem.ShoppingCartType;
        }

        //quantity
        model.EnteredQuantity = updatecartitem != null ? updatecartitem.Quantity : product.OrderMinimumQuantity;
        //allowed quantities
        var allowedQuantities = _productService.ParseAllowedQuantities(product);
        foreach (var qty in allowedQuantities)
        {
            model.AllowedQuantities.Add(new SelectListItemDto
            {
                Text = qty.ToString(),
                Value = qty.ToString(),
                Selected = updatecartitem != null && updatecartitem.Quantity == qty
            });
        }
        //minimum quantity notification
        if (product.OrderMinimumQuantity > 1)
        {
            model.MinimumQuantityNotification = string.Format(await _localizationService.GetResourceAsync("Products.MinimumQuantityNotification"), product.OrderMinimumQuantity);
        }

        //'add to cart', 'add to wishlist' buttons
        model.DisableBuyButton = product.DisableBuyButton || !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART);
        model.DisableWishlistButton = product.DisableWishlistButton || !await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST);
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
        {
            model.DisableBuyButton = true;
            model.DisableWishlistButton = true;
        }
        //pre-order
        if (product.AvailableForPreOrder)
        {
            model.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
                                         product.PreOrderAvailabilityStartDateTimeUtc.Value >= DateTime.UtcNow;
            model.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;

            if (model.AvailableForPreOrder &&
                model.PreOrderAvailabilityStartDateTimeUtc.HasValue &&
                _catalogSettings.DisplayDatePreOrderAvailability)
            {
                model.PreOrderAvailabilityStartDateTimeUserTime =
                    (await _dateTimeHelper.ConvertToUserTimeAsync(model.PreOrderAvailabilityStartDateTimeUtc.Value)).ToString("D");
            }
        }

        return model;
    }

    /// <summary>
    /// Prepare the product attribute models
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="updatecartitem">Updated shopping cart item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of product attribute model
    /// </returns>
    protected virtual async Task<IList<ProductAttributeOverviewDto>> PrepareProductAttributeDtosAsync(Product product, ShoppingCartItem updatecartitem)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = new List<ProductAttributeOverviewDto>();
        var store = updatecartitem != null ? await _storeService.GetStoreByIdAsync(updatecartitem.StoreId) : await _storeContext.GetCurrentStoreAsync();

        var productAttributeMapping = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(product.Id);
        foreach (var attribute in productAttributeMapping)
        {
            var productAttribute = await _productAttributeService.GetProductAttributeByIdAsync(attribute.ProductAttributeId);

            var attributeModel = new ProductAttributeOverviewDto
            {
                Id = attribute.Id,
                ProductId = product.Id,
                ProductAttributeId = attribute.ProductAttributeId,
                Name = await _localizationService.GetLocalizedAsync(productAttribute, x => x.Name),
                Description = await _localizationService.GetLocalizedAsync(productAttribute, x => x.Description),
                TextPrompt = await _localizationService.GetLocalizedAsync(attribute, x => x.TextPrompt),
                IsRequired = attribute.IsRequired,
                AttributeControlType = attribute.AttributeControlType,
                DefaultValue = updatecartitem != null ? null : await _localizationService.GetLocalizedAsync(attribute, x => x.DefaultValue),
                HasCondition = !string.IsNullOrEmpty(attribute.ConditionAttributeXml)
            };
            if (!string.IsNullOrEmpty(attribute.ValidationFileAllowedExtensions))
            {
                attributeModel.AllowedFileExtensions = attribute.ValidationFileAllowedExtensions
                    .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }

            if (attribute.ShouldHaveValues())
            {
                //values
                var attributeValues = await _productAttributeService.GetProductAttributeValuesAsync(attribute.Id);
                foreach (var attributeValue in attributeValues)
                {
                    var valueModel = new ProductAttributeValueDto
                    {
                        Id = attributeValue.Id,
                        Name = await _localizationService.GetLocalizedAsync(attributeValue, x => x.Name),
                        ColorSquaresRgb = attributeValue.ColorSquaresRgb, //used with "Color squares" attribute type
                        IsPreSelected = attributeValue.IsPreSelected,
                        CustomerEntersQty = attributeValue.CustomerEntersQty,
                        Quantity = attributeValue.Quantity
                    };
                    attributeModel.Values.Add(valueModel);

                    //display price if allowed
                    if (await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
                    {
                        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
                        var customer = updatecartitem?.CustomerId is null ? currentCustomer : await _customerService.GetCustomerByIdAsync(updatecartitem.CustomerId);

                        var attributeValuePriceAdjustment = await _priceCalculationService.GetProductAttributeValuePriceAdjustmentAsync(product, attributeValue, customer, store, quantity: updatecartitem?.Quantity ?? 1);
                        var (priceAdjustmentBase, _) = await _taxService.GetProductPriceAsync(product, attributeValuePriceAdjustment);
                        var priceAdjustment = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(priceAdjustmentBase, await _workContext.GetWorkingCurrencyAsync());

                        if (attributeValue.PriceAdjustmentUsePercentage)
                        {
                            var priceAdjustmentStr = attributeValue.PriceAdjustment.ToString("G29");
                            if (attributeValue.PriceAdjustment > decimal.Zero)
                                valueModel.PriceAdjustment = "+";
                            valueModel.PriceAdjustment += priceAdjustmentStr + "%";
                        }
                        else
                        {
                            if (priceAdjustmentBase > decimal.Zero)
                                valueModel.PriceAdjustment = "+" + await _priceFormatter.FormatPriceAsync(priceAdjustment, false, false);
                            else if (priceAdjustmentBase < decimal.Zero)
                                valueModel.PriceAdjustment = "-" + await _priceFormatter.FormatPriceAsync(-priceAdjustment, false, false);
                        }

                        valueModel.PriceAdjustmentValue = priceAdjustment;
                    }

                    //"image square" picture (with with "image squares" attribute type only)
                    if (attributeValue.ImageSquaresPictureId > 0)
                    {
                        var productAttributeImageSquarePictureCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductAttributeImageSquarePictureDtoKey
                            , attributeValue.ImageSquaresPictureId,
                            _webHelper.IsCurrentConnectionSecured(),
                            await _storeContext.GetCurrentStoreAsync());
                        valueModel.ImageSquaresPicture = await _staticCacheManager.GetAsync(productAttributeImageSquarePictureCacheKey, async () =>
                        {
                            var imageSquaresPicture = await _pictureService.GetPictureByIdAsync(attributeValue.ImageSquaresPictureId);

                            (var fullSizeImageUrl, imageSquaresPicture) = await _pictureService.GetPictureUrlAsync(imageSquaresPicture);
                            (var imageUrl, imageSquaresPicture) = await _pictureService.GetPictureUrlAsync(imageSquaresPicture, _mediaSettings.ImageSquarePictureSize);

                            if (imageSquaresPicture != null)
                            {

                                return new PictureDto
                                {
                                    FullSizeImageUrl = fullSizeImageUrl,
                                    ImageUrl = imageUrl
                                };
                            }

                            return new PictureDto();
                        });
                    }

                    //picture of a product attribute value
                    valueModel.PictureId = (await _productAttributeService.GetProductAttributeValuePicturesAsync(attributeValue.Id)).FirstOrDefault()?.PictureId ?? 0;
                }
            }

            //set already selected attributes (if we're going to update the existing shopping cart item)
            if (updatecartitem != null)
            {
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.Checkboxes:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                        {
                            if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                            {
                                //clear default selection
                                foreach (var item in attributeModel.Values)
                                    item.IsPreSelected = false;

                                //select new values
                                var selectedValues = await _productAttributeParser.ParseProductAttributeValuesAsync(updatecartitem.AttributesXml);
                                foreach (var attributeValue in selectedValues)
                                    foreach (var item in attributeModel.Values)
                                        if (attributeValue.Id == item.Id)
                                        {
                                            item.IsPreSelected = true;

                                            //set customer entered quantity
                                            if (attributeValue.CustomerEntersQty)
                                                item.Quantity = attributeValue.Quantity;
                                        }
                            }
                        }

                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //values are already pre-set

                            //set customer entered quantity
                            if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                            {
                                foreach (var attributeValue in (await _productAttributeParser.ParseProductAttributeValuesAsync(updatecartitem.AttributesXml))
                                         .Where(value => value.CustomerEntersQty))
                                {
                                    var item = attributeModel.Values.FirstOrDefault(value => value.Id == attributeValue.Id);
                                    if (item != null)
                                        item.Quantity = attributeValue.Quantity;
                                }
                            }
                        }

                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                            {
                                var enteredText = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
                                if (enteredText.Any())
                                    attributeModel.DefaultValue = enteredText[0];
                            }
                        }

                        break;
                    case AttributeControlType.Datepicker:
                        {
                            //keep in mind my that the code below works only in the current culture
                            var selectedDateStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
                            if (selectedDateStr.Any())
                            {
                                if (DateTime.TryParseExact(selectedDateStr[0], "D", CultureInfo.CurrentCulture, DateTimeStyles.None, out var selectedDate))
                                {
                                    //successfully parsed
                                    attributeModel.SelectedDay = selectedDate.Day;
                                    attributeModel.SelectedMonth = selectedDate.Month;
                                    attributeModel.SelectedYear = selectedDate.Year;
                                }
                            }
                        }

                        break;
                    case AttributeControlType.FileUpload:
                        {
                            if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                            {
                                var downloadGuidStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id).FirstOrDefault();
                                _ = Guid.TryParse(downloadGuidStr, out var downloadGuid);
                                var download = await _downloadService.GetDownloadByGuidAsync(downloadGuid);
                                if (download != null)
                                    attributeModel.DefaultValue = download.DownloadGuid.ToString();
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            model.Add(attributeModel);
        }

        return model;
    }

    /// <summary>
    /// Prepare the product tier price models
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of tier price model
    /// </returns>
    protected virtual async Task<IList<TierPriceDto>> PrepareProductTierPriceDtosAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var model = await (await _productService.GetTierPricesAsync(product, customer, store))
            .SelectAwait(async tierPrice =>
            {
                var priceBase = (await _taxService.GetProductPriceAsync(product, (await _priceCalculationService.GetFinalPriceAsync(product,
                    customer, store, decimal.Zero, _catalogSettings.DisplayTierPricesWithDiscounts,
                    tierPrice.Quantity)).finalPrice)).price;

                var price = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(priceBase, await _workContext.GetWorkingCurrencyAsync());

                return new TierPriceDto
                {
                    Quantity = tierPrice.Quantity,
                    Price = await _priceFormatter.FormatPriceAsync(price, false, false),
                    PriceValue = price
                };
            }).ToListAsync();

        return model;
    }

    /// <summary>
    /// Prepare the product manufacturer models
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of manufacturer brief info model
    /// </returns>
    protected virtual async Task<IList<ManufacturerBriefInfoDto>> PrepareProductManufacturerDtosAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = await (await _manufacturerService.GetProductManufacturersByProductIdAsync(product.Id))
            .SelectAwait(async pm =>
            {
                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(pm.ManufacturerId);
                var modelMan = new ManufacturerBriefInfoDto
                {
                    Id = manufacturer.Id,
                    Name = await _localizationService.GetLocalizedAsync(manufacturer, x => x.Name),
                    SeName = await _urlRecordService.GetSeNameAsync(manufacturer)
                };

                return modelMan;
            }).ToListAsync();

        return model;
    }

    /// <summary>
    /// Prepare the product details picture model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the picture model for the default picture; All picture models
    /// </returns>
    protected virtual async Task<(PictureDto pictureModel, IList<PictureDto> allPictureDtos, IList<VideoDto> allVideoDtos)> PrepareProductDetailsPictureDtoAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        //default picture size
        var defaultPictureSize = _mediaSettings.ProductDetailsPictureSize;

        //prepare picture models
        var productPicturesCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductDetailsPicturesModelKey
            , product, defaultPictureSize,
            await _workContext.GetWorkingLanguageAsync(), _webHelper.IsCurrentConnectionSecured(), await _storeContext.GetCurrentStoreAsync());
        var cachedPictures = await _staticCacheManager.GetAsync(productPicturesCacheKey, async () =>
        {
            var productName = await _localizationService.GetLocalizedAsync(product, x => x.Name);

            var pictures = await _pictureService.GetPicturesByProductIdAsync(product.Id);
            var defaultPicture = pictures.FirstOrDefault();

            (var fullSizeImageUrl, defaultPicture) = await _pictureService.GetPictureUrlAsync(defaultPicture, 0);
            (var imageUrl, defaultPicture) = await _pictureService.GetPictureUrlAsync(defaultPicture, defaultPictureSize);

            var defaultPictureDto = new PictureDto
            {
                ImageUrl = imageUrl,
                FullSizeImageUrl = fullSizeImageUrl,
                //"title" attribute
                Title = (defaultPicture != null && !string.IsNullOrEmpty(defaultPicture.TitleAttribute)) ?
                    defaultPicture.TitleAttribute :
                    string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageLinkTitleFormat.Details"), productName),
                //"alt" attribute
                AlternateText = (defaultPicture != null && !string.IsNullOrEmpty(defaultPicture.AltAttribute)) ?
                    defaultPicture.AltAttribute :
                    string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageAlternateTextFormat.Details"), productName)
            };

            //all pictures
            var pictureModels = new List<PictureDto>();
            for (var i = 0; i < pictures.Count; i++)
            {
                var picture = pictures[i];

                (fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                (imageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture, defaultPictureSize);
                (var thumbImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture, _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage);

                var pictureModel = new PictureDto
                {
                    Id = picture.Id,
                    ImageUrl = imageUrl,
                    ThumbImageUrl = thumbImageUrl,
                    FullSizeImageUrl = fullSizeImageUrl,
                    Title = string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageLinkTitleFormat.Details"), productName),
                    AlternateText = string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageAlternateTextFormat.Details"), productName),
                };
                //"title" attribute
                pictureModel.Title = !string.IsNullOrEmpty(picture.TitleAttribute) ?
                    picture.TitleAttribute :
                    string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageLinkTitleFormat.Details"), productName);
                //"alt" attribute
                pictureModel.AlternateText = !string.IsNullOrEmpty(picture.AltAttribute) ?
                    picture.AltAttribute :
                    string.Format(await _localizationService.GetResourceAsync("Media.Product.ImageAlternateTextFormat.Details"), productName);

                pictureModels.Add(pictureModel);
            }

            return new { DefaultPictureDto = defaultPictureDto, PictureDtos = pictureModels };
        });

        var allPictureDtos = cachedPictures.PictureDtos;

        //all videos
        var allvideoModels = new List<VideoDto>();
        var videos = await _videoService.GetVideosByProductIdAsync(product.Id);
        foreach (var video in videos)
        {
            var videoModel = new VideoDto
            {
                VideoUrl = video.VideoUrl,
                //Allow = _mediaSettings.VideoIframeAllow,
                //Width = _mediaSettings.VideoIframeWidth,
                //Height = _mediaSettings.VideoIframeHeight
            };

            allvideoModels.Add(videoModel);
        }
        return (cachedPictures.DefaultPictureDto, allPictureDtos, allvideoModels);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the product template view path
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view path
    /// </returns>
    public virtual async Task<string> PrepareProductTemplateViewPathAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var template = (await _productTemplateService.GetProductTemplateByIdAsync(product.ProductTemplateId) ??
                        (await _productTemplateService.GetAllProductTemplatesAsync()).FirstOrDefault()) ?? throw new Exception("No default template could be loaded");

        return template.ViewPath;
    }

    /// <summary>
    /// Prepare the product overview models
    /// </summary>
    /// <param name="products">Collection of products</param>
    /// <param name="preparePriceModel">Whether to prepare the price model</param>
    /// <param name="preparePictureDto">Whether to prepare the picture model</param>
    /// <param name="productThumbPictureSize">Product thumb picture size (longest side); pass null to use the default value of media settings</param>
    /// <param name="prepareSpecificationAttributes">Whether to prepare the specification attribute models</param>
    /// <param name="forceRedirectionAfterAddingToCart">Whether to force redirection after adding to cart</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the collection of product overview model
    /// </returns>
    public virtual async Task<IEnumerable<ProductOverviewDto>> PrepareProductOverviewDtosAsync(IEnumerable<Product> products,
        bool preparePriceModel = true, bool preparePictureDto = true,
        int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
        bool forceRedirectionAfterAddingToCart = false)
    {
        ArgumentNullException.ThrowIfNull(products);

        var models = new List<ProductOverviewDto>();
        foreach (var product in products)
        {
            var model = new ProductOverviewDto
            {
                Id = product.Id,
                Name = await _localizationService.GetLocalizedAsync(product, x => x.Name),
                ShortDescription = await _localizationService.GetLocalizedAsync(product, x => x.ShortDescription),
                FullDescription = await _localizationService.GetLocalizedAsync(product, x => x.FullDescription),
                SeName = await _urlRecordService.GetSeNameAsync(product),
                Sku = product.Sku,
            };

            //price
            if (preparePriceModel)
            {
                model.ProductPrice = await PrepareProductOverviewPriceModelAsync(product, forceRedirectionAfterAddingToCart);
            }

            //picture
            if (preparePictureDto)
            {
                model.Pictures = await PrepareProductOverviewPicturesModelAsync(product, productThumbPictureSize);
            }

            //specs
            if (prepareSpecificationAttributes)
            {
                model.ProductSpecification = await PrepareProductSpecificationDtoAsync(product);
            }

            //reviews
            model.ReviewOverview = await PrepareProductReviewOverviewModelAsync(product);

            models.Add(model);
        }

        return models;
    }

    /// <summary>
    /// Prepare the product combination models
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product combination models
    /// </returns>
    public virtual async Task<IList<ProductCombinationDto>> PrepareProductCombinationDtosAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var result = new List<ProductCombinationDto>();

        var combinations = await _productAttributeService
            .GetAllProductAttributeCombinationsAsync(product.Id);
        if (combinations?.Any() == true)
        {
            foreach (var combination in combinations)
            {
                var combinationModel = new ProductCombinationDto
                {
                    InStock = combination.StockQuantity > 0 || combination.AllowOutOfStockOrders
                };

                var mappings = await _productAttributeParser
                    .ParseProductAttributeMappingsAsync(combination.AttributesXml);
                if (mappings == null || !mappings.Any())
                    continue;

                foreach (var mapping in mappings)
                {
                    var attributeModel = new ProductAttributeDto
                    {
                        Id = mapping.Id
                    };

                    var values = await _productAttributeParser
                        .ParseProductAttributeValuesAsync(combination.AttributesXml, mapping.Id);
                    if (values == null || !values.Any())
                        continue;

                    foreach (var value in values)
                        attributeModel.ValueIds.Add(value.Id);

                    combinationModel.Attributes.Add(attributeModel);
                }

                result.Add(combinationModel);
            }
        }

        return result;
    }

    /// <summary>
    /// Prepare the product details model
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="updatecartitem">Updated shopping cart item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product details model
    /// </returns>
    public virtual async Task<ProductDetailsDto> PrepareProductDetailsDtoAsync(Product product,
        ShoppingCartItem updatecartitem = null)
    {
        ArgumentNullException.ThrowIfNull(product);

        //standard properties
        var model = new ProductDetailsDto
        {
            Id = product.Id,
            Name = await _localizationService.GetLocalizedAsync(product, x => x.Name),
            ShortDescription = await _localizationService.GetLocalizedAsync(product, x => x.ShortDescription),
            FullDescription = await _localizationService.GetLocalizedAsync(product, x => x.FullDescription),
            SeName = await _urlRecordService.GetSeNameAsync(product),
            ShowSku = _catalogSettings.ShowSkuOnProductDetailsPage,
            Sku = product.Sku,
            ShowManufacturerPartNumber = _catalogSettings.ShowManufacturerPartNumber,
            FreeShippingNotificationEnabled = _catalogSettings.ShowFreeShippingNotification,
            ManufacturerPartNumber = product.ManufacturerPartNumber,
            ShowGtin = _catalogSettings.ShowGtin,
            Gtin = product.Gtin,
            StockAvailability = await _productService.FormatStockMessageAsync(product),
            DisplayDiscontinuedMessage = !product.Published && _catalogSettings.DisplayDiscontinuedMessageForUnpublishedProducts,
            AvailableEndDate = product.AvailableEndDateTimeUtc,
            DisplayAttributeCombinationImagesOnly = product.DisplayAttributeCombinationImagesOnly
        };


        //shipping info
        model.IsShipEnabled = product.IsShipEnabled;
        if (product.IsShipEnabled)
        {
            model.IsFreeShipping = product.IsFreeShipping;
            //delivery date
            var deliveryDate = await _dateRangeService.GetDeliveryDateByIdAsync(product.DeliveryDateId);
            if (deliveryDate != null)
            {
                model.DeliveryDate = await _localizationService.GetLocalizedAsync(deliveryDate, dd => dd.Name);
            }
        }

        var store = await _storeContext.GetCurrentStoreAsync();
        //email a friend
        model.EmailAFriendEnabled = _catalogSettings.EmailAFriendEnabled;
        //compare products
        model.CompareProductsEnabled = _catalogSettings.CompareProductsEnabled;
        //store name
        model.CurrentStoreName = await _localizationService.GetLocalizedAsync(store, x => x.Name);

        //vendor details
        if (_vendorSettings.ShowVendorOnProductDetailsPage)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(product.VendorId);
            if (vendor != null && !vendor.Deleted && vendor.Active)
            {
                model.ShowVendor = true;

                model.Vendor = new VendorBriefInfoDto
                {
                    Id = vendor.Id,
                    Name = await _localizationService.GetLocalizedAsync(vendor, x => x.Name),
                    SeName = await _urlRecordService.GetSeNameAsync(vendor),
                };
            }
        }

        //page sharing
        if (_catalogSettings.ShowShareButton && !string.IsNullOrEmpty(_catalogSettings.PageShareCode))
        {
            var shareCode = _catalogSettings.PageShareCode;
            if (_webHelper.IsCurrentConnectionSecured())
            {
                //need to change the add this link to be https linked when the page is, so that the page doesn't ask about mixed mode when viewed in https...
                shareCode = shareCode.Replace("http://", "https://");
            }

            model.PageShareCode = shareCode;
        }

  
        model.InStock = product.BackorderMode != BackorderMode.NoBackorders
                        || product.StockQuantity > 0;
        model.DisplayBackInStockSubscription = !model.InStock && product.AllowBackInStockSubscriptions;


        //breadcrumb
        //do not prepare this model for the associated products. anyway it's not used
        if (_catalogSettings.CategoryBreadcrumbEnabled)
        {
            model.Breadcrumb = await PrepareProductBreadcrumbModelAsync(product);
        }


        model.ProductTags = await PrepareProductTagDtosAsync(product);
        

        //pictures and videos
        model.DefaultPictureZoomEnabled = _mediaSettings.DefaultPictureZoomEnabled;
        IList<PictureDto> allPictureDtos;
        IList<VideoDto> allVideoDtos;
        (model.DefaultPicture, allPictureDtos, allVideoDtos) = await PrepareProductDetailsPictureDtoAsync(product);
        model.Pictures = allPictureDtos;
        model.Videos = allVideoDtos;

        //price
        model.ProductPrice = await PrepareProductPriceModelAsync(product);

        //'Add to cart' model
        model.AddToCart = await PrepareProductAddToCartModelAsync(product, updatecartitem);
        var customer = await _workContext.GetCurrentCustomerAsync();

        //product attributes
        model.ProductAttributes = await PrepareProductAttributeDtosAsync(product, updatecartitem);


        model.ProductSpecification = await PrepareProductSpecificationDtoAsync(product);
        

        //product review overview
        model.ProductReviewOverview = await PrepareProductReviewOverviewModelAsync(product);

        model.ProductReviews = await PrepareProductReviewsModelAsync(product);

        //tier prices
        if (await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
        {
            model.TierPrices = await PrepareProductTierPriceDtosAsync(product);
        }

        //manufacturers
        model.ProductManufacturers = await PrepareProductManufacturerDtosAsync(product);

        //estimate shipping
        if (_shippingSettings.EstimateShippingProductPageEnabled && !model.IsFreeShipping)
        {
            var wrappedProduct = new ShoppingCartItem
            {
                StoreId = store.Id,
                ShoppingCartTypeId = (int)ShoppingCartType.ShoppingCart,
                CustomerId = customer.Id,
                ProductId = product.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            var estimateShippingModel = await _shoppingCartModelFactory.PrepareEstimateShippingDtoAsync(new[] { wrappedProduct });

            model.ProductEstimateShipping.ProductId = product.Id;
            model.ProductEstimateShipping.RequestDelay = estimateShippingModel.RequestDelay;
            model.ProductEstimateShipping.Enabled = estimateShippingModel.Enabled;
            model.ProductEstimateShipping.CountryId = estimateShippingModel.CountryId;
            model.ProductEstimateShipping.StateProvinceId = estimateShippingModel.StateProvinceId;
            model.ProductEstimateShipping.ZipPostalCode = estimateShippingModel.ZipPostalCode;
            model.ProductEstimateShipping.UseCity = estimateShippingModel.UseCity;
            model.ProductEstimateShipping.City = estimateShippingModel.City;
            model.ProductEstimateShipping.AvailableCountries = estimateShippingModel.AvailableCountries;
            model.ProductEstimateShipping.AvailableStates = estimateShippingModel.AvailableStates;
        }

        if (_seoSettings.MicrodataEnabled)
        {
            var jsonLdModel = await _jsonLdModelFactory.PrepareJsonLdProductAsync(model);
            model.JsonLd = JsonConvert.SerializeObject(jsonLdModel, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        return model;
    }

    /// <summary>
    /// Prepare the product reviews model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product reviews model
    /// </returns>
    public virtual async Task<ProductReviewsDto> PrepareProductReviewsModelAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = new ProductReviewsDto
        {
            ProductId = product.Id
        };

        var currentStore = await _storeContext.GetCurrentStoreAsync();

        var productReviews = await _productReviewService.GetAllProductReviewsAsync(
            approved: true,
            productId: product.Id,
            storeId: _catalogSettings.ShowProductReviewsPerStore ? currentStore.Id : 0);

        //get all review types
        foreach (var reviewType in await _reviewTypeService.GetAllReviewTypesAsync())
        {
            model.ReviewTypeList.Add(new ReviewTypeDto
            {
                Id = reviewType.Id,
                Name = await _localizationService.GetLocalizedAsync(reviewType, entity => entity.Name),
                Description = await _localizationService.GetLocalizedAsync(reviewType, entity => entity.Description),
                VisibleToAllCustomers = reviewType.VisibleToAllCustomers,
                DisplayOrder = reviewType.DisplayOrder,
                IsRequired = reviewType.IsRequired,
            });
        }

        var currentCustomer = await _workContext.GetCurrentCustomerAsync();

        //filling data from db
        foreach (var pr in productReviews)
        {
            var customer = await _customerService.GetCustomerByIdAsync(pr.CustomerId);

            var productReviewModel = new ProductReviewDto
            {
                Id = pr.Id,
                CustomerId = pr.CustomerId,
                CustomerName = await _customerService.FormatUsernameAsync(customer),
                AllowViewingProfiles = _customerSettings.AllowViewingProfiles && customer != null && !await _customerService.IsGuestAsync(customer),
                Title = pr.Title,
                ReviewText = pr.ReviewText,
                ReplyText = pr.ReplyText,
                Rating = pr.Rating,
                Helpfulness = new ProductReviewHelpfulnessModel
                {
                    ProductReviewId = pr.Id,
                    HelpfulYesTotal = pr.HelpfulYesTotal,
                    HelpfulNoTotal = pr.HelpfulNoTotal,
                },
                WrittenOnStr = (await _dateTimeHelper.ConvertToUserTimeAsync(pr.CreatedOnUtc, DateTimeKind.Utc)).ToString("g"),
            };

            if (_customerSettings.AllowCustomersToUploadAvatars)
            {
                productReviewModel.CustomerAvatarUrl = await _pictureService.GetPictureUrlAsync(
                    await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.AvatarPictureIdAttribute),
                    _mediaSettings.AvatarPictureSize, _customerSettings.DefaultAvatarEnabled, defaultPictureType: PictureType.Avatar);
            }

            foreach (var q in await _reviewTypeService.GetProductReviewReviewTypeMappingsByProductReviewIdAsync(pr.Id))
            {
                var reviewType = await _reviewTypeService.GetReviewTypeByIdAsync(q.ReviewTypeId);

                productReviewModel.AdditionalProductReviewList.Add(new ProductReviewReviewTypeMappingDto
                {
                    ReviewTypeId = q.ReviewTypeId,
                    ProductReviewId = pr.Id,
                    Rating = q.Rating,
                    Name = await _localizationService.GetLocalizedAsync(reviewType, x => x.Name),
                    VisibleToAllCustomers = reviewType.VisibleToAllCustomers || currentCustomer.Id == pr.CustomerId,
                });
            }

            model.Items.Add(productReviewModel);
        }

        foreach (var rt in model.ReviewTypeList)
        {
            if (model.ReviewTypeList.Count <= model.AddAdditionalProductReviewList.Count)
                continue;
            var reviewType = await _reviewTypeService.GetReviewTypeByIdAsync(rt.Id);
            var reviewTypeMappingModel = new AddProductReviewReviewTypeMappingDto
            {
                ReviewTypeId = rt.Id,
                Name = await _localizationService.GetLocalizedAsync(reviewType, entity => entity.Name),
                Description = await _localizationService.GetLocalizedAsync(reviewType, entity => entity.Description),
                DisplayOrder = rt.DisplayOrder,
                IsRequired = rt.IsRequired,
            };

            model.AddAdditionalProductReviewList.Add(reviewTypeMappingModel);
        }

        //Average rating
        foreach (var rtm in model.ReviewTypeList)
        {
            var totalRating = 0;
            var totalCount = 0;
            foreach (var item in model.Items)
            {
                foreach (var q in item.AdditionalProductReviewList.Where(w => w.ReviewTypeId == rtm.Id))
                {
                    totalRating += q.Rating;
                    totalCount = ++totalCount;
                }
            }

            rtm.AverageRating = (double)totalRating / (totalCount > 0 ? totalCount : 1);
        }

        model.AddProductReview.CanCurrentCustomerLeaveReview = _catalogSettings.AllowAnonymousUsersToReviewProduct || !await _customerService.IsGuestAsync(currentCustomer);
        model.AddProductReview.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnProductReviewPage;
        model.AddProductReview.CanAddNewReview = await _productReviewService.CanAddReviewAsync(product.Id, _catalogSettings.ShowProductReviewsPerStore ? currentStore.Id : 0);

        return model;
    }

    /// <summary>
    /// Prepare the customer product reviews model
    /// </summary>
    /// <param name="page">Number of items page; pass null to load the first page</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer product reviews model
    /// </returns>
    public virtual async Task<CustomerProductReviewsDto> PrepareCustomerProductReviewsModelAsync(int? page)
    {
        var pageSize = _catalogSettings.ProductReviewsPageSizeOnAccountPage;
        var pageIndex = 0;

        if (page > 0)
        {
            pageIndex = page.Value - 1;
        }

        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();

        var list = await _productReviewService.GetAllProductReviewsAsync(
            customerId: customer.Id,
            approved: null,
            storeId: _catalogSettings.ShowProductReviewsPerStore ? store.Id : 0,
            pageIndex: pageIndex,
            pageSize: pageSize);

        var productReviews = new List<CustomerProductReviewDto>();

        foreach (var review in list)
        {
            var product = await _productService.GetProductByIdAsync(review.ProductId);

            var productReviewModel = new CustomerProductReviewDto
            {
                Title = review.Title,
                ProductId = product.Id,
                ProductName = await _localizationService.GetLocalizedAsync(product, p => p.Name),
                ProductSeName = await _urlRecordService.GetSeNameAsync(product),
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReplyText = review.ReplyText,
                WrittenOnStr = (await _dateTimeHelper.ConvertToUserTimeAsync(review.CreatedOnUtc, DateTimeKind.Utc)).ToString("g")
            };

            if (_catalogSettings.ProductReviewsMustBeApproved)
            {
                productReviewModel.ApprovalStatus = review.IsApproved
                    ? await _localizationService.GetResourceAsync("Account.CustomerProductReviews.ApprovalStatus.Approved")
                    : await _localizationService.GetResourceAsync("Account.CustomerProductReviews.ApprovalStatus.Pending");
            }

            foreach (var q in await _reviewTypeService.GetProductReviewReviewTypeMappingsByProductReviewIdAsync(review.Id))
            {
                var reviewType = await _reviewTypeService.GetReviewTypeByIdAsync(q.ReviewTypeId);

                productReviewModel.AdditionalProductReviewList.Add(new ProductReviewReviewTypeMappingDto
                {
                    ReviewTypeId = q.ReviewTypeId,
                    ProductReviewId = review.Id,
                    Rating = q.Rating,
                    Name = await _localizationService.GetLocalizedAsync(reviewType, x => x.Name),
                });
            }

            productReviews.Add(productReviewModel);
        }

        var pagerModel = new PagerDto(_localizationService)
        {
            PageSize = list.PageSize,
            TotalRecords = list.TotalCount,
            PageIndex = list.PageIndex,
            ShowTotalSummary = false,
            RouteActionName = "CustomerProductReviewsPaged",
            UseRouteLinks = true,
            RouteValues = new CustomerProductReviewsDto.CustomerProductReviewsRouteValues { PageNumber = pageIndex }
        };

        var model = new CustomerProductReviewsDto
        {
            ProductReviews = productReviews,
            Pager = pagerModel
        };

        return model;
    }

    /// <summary>
    /// Prepare the product email a friend model
    /// </summary>
    /// <param name="model">Product email a friend model</param>
    /// <param name="product">Product</param>
    /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product email a friend model
    /// </returns>
    public virtual async Task<ProductEmailAFriendDto> PrepareProductEmailAFriendDtoAsync(ProductEmailAFriendDto model, Product product, bool excludeProperties)
    {
        ArgumentNullException.ThrowIfNull(model);

        ArgumentNullException.ThrowIfNull(product);

        model.ProductId = product.Id;
        model.ProductName = await _localizationService.GetLocalizedAsync(product, x => x.Name);
        model.ProductSeName = await _urlRecordService.GetSeNameAsync(product);
        model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnEmailProductToFriendPage;
        if (!excludeProperties)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            model.YourEmailAddress = customer.Email;
        }

        return model;
    }

    /// <summary>
    /// Prepare the product specification model
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product specification model
    /// </returns>
    public virtual async Task<ProductSpecificationDto> PrepareProductSpecificationDtoAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var model = new ProductSpecificationDto();

        // Add non-grouped attributes first
        model.Groups.Add(new ProductSpecificationAttributeGroupDto
        {
            Attributes = await PrepareProductSpecificationAttributeDtoAsync(product, null)
        });

        // Add grouped attributes
        var groups = await _specificationAttributeService.GetProductSpecificationAttributeGroupsAsync(product.Id);
        foreach (var group in groups)
        {
            model.Groups.Add(new ProductSpecificationAttributeGroupDto
            {
                Id = group.Id,
                Name = await _localizationService.GetLocalizedAsync(group, x => x.Name),
                Attributes = await PrepareProductSpecificationAttributeDtoAsync(product, group)
            });
        }

        return model;
    }

    #endregion
}