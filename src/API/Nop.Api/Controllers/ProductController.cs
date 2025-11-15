using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Api.Factories;
using Nop.Api.Framework;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.DTOs.Catalog;
using System.Net;
using Nop.Api.DTOs.Responses;
using Nop.Api.DTOs.ShoppingCart;
using Nop.Api.Infrastructure.Cache;
using Nop.Core.Caching;
using Nop.Api.DTOs.Common;

namespace Nop.Api.Controllers;

public partial class ProductController : BasePublicController
{
    private readonly IProductReviewService _productReviewService;
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CatalogSettings _catalogSettings;
    protected readonly IAclService _aclService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILocalizationService _localizationService;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly INotificationService _notificationService;
    protected readonly IOrderService _orderService;
    protected readonly IPermissionService _permissionService;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductDtoFactory _productModelFactory;
    protected readonly IProductService _productService;
    protected readonly IReviewTypeService _reviewTypeService;
    protected readonly IShoppingCartDtoFactory _shoppingCartModelFactory;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly ShoppingCartSettings _shoppingCartSettings;
    protected readonly ShippingSettings _shippingSettings;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IOrderReportService _orderReportService;

    #endregion

    #region Ctor

    public ProductController(IProductReviewService productReviewService, CaptchaSettings captchaSettings,
        CatalogSettings catalogSettings,
        IAclService aclService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IEventPublisher eventPublisher,
        IHtmlFormatter htmlFormatter,
        ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        INotificationService notificationService,
        IOrderService orderService,
        IPermissionService permissionService,
        IProductAttributeParser productAttributeParser,
        IProductDtoFactory productModelFactory,
        IProductService productService,
        IReviewTypeService reviewTypeService,
        IShoppingCartDtoFactory shoppingCartModelFactory,
        IShoppingCartService shoppingCartService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        ShoppingCartSettings shoppingCartSettings,
        ShippingSettings shippingSettings,
        IStaticCacheManager staticCacheManager,
        IOrderReportService orderReportService)
    {
        _productReviewService = productReviewService;
        _captchaSettings = captchaSettings;
        _catalogSettings = catalogSettings;
        _aclService = aclService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _eventPublisher = eventPublisher;
        _htmlFormatter = htmlFormatter;
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _notificationService = notificationService;
        _orderService = orderService;
        _permissionService = permissionService;
        _productAttributeParser = productAttributeParser;
        _productModelFactory = productModelFactory;
        _productService = productService;
        _reviewTypeService = reviewTypeService;
        _shoppingCartModelFactory = shoppingCartModelFactory;
        _shoppingCartService = shoppingCartService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _urlRecordService = urlRecordService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _shoppingCartSettings = shoppingCartSettings;
        _shippingSettings = shippingSettings;
        _staticCacheManager = staticCacheManager;
        _orderReportService = orderReportService;
    }

    #endregion

    #region Utilities

    protected virtual async Task ValidateProductReviewAvailabilityAsync(Product product)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Reviews.OnlyRegisteredUsersCanWriteReviews"));

        if (!_catalogSettings.ProductReviewPossibleOnlyAfterPurchasing)
            return;

        var hasCompletedOrders =  await HasCompletedOrdersAsync(product);

        if (!hasCompletedOrders)
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Reviews.ProductReviewPossibleOnlyAfterPurchasing"));
    }

    protected virtual async ValueTask<bool> HasCompletedOrdersAsync(Product product)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        return (await _orderService.SearchOrdersAsync(customerId: customer.Id,
                productId: product.Id,
                osIds: [(int)OrderStatus.Complete],
            pageSize: 1)).Any();
    }

    #endregion

    #region Product details page

    [HttpGet]
    [Route("GetProductDetails/{productId}", Name = "GetProductDetails")]
    [ProducesResponseType(typeof(ProductDetailsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductDetails([FromRoute] int productId, [FromQuery] int updatecartitemid = 0)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null || product.Deleted)
            return InvokeHttp404();

        var notAvailable =
            //published?
            (!product.Published && !_catalogSettings.AllowViewUnpublishedProductPage) ||
            //ACL (access control list) 
            !await _aclService.AuthorizeAsync(product) ||
            //Store mapping
            !await _storeMappingService.AuthorizeAsync(product) ||
            //availability dates
            !_productService.ProductIsAvailable(product);
        //Check whether the current user has a "Manage products" permission (usually a store owner)
        //We should allows him (her) to use "Preview" functionality
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE);
        if (notAvailable && !hasAdminAccess)
            return InvokeHttp404();


        //update existing shopping cart?
        ShoppingCartItem updatecartitem = null;
        if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
        {
            var seName = await _urlRecordService.GetSeNameAsync(product);
            var productUrl = await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = seName });
            var store = await _storeContext.GetCurrentStoreAsync();
            var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), storeId: store.Id);
            updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);

            //not found?
            if (updatecartitem == null)
                return LocalRedirect(productUrl);

            //is it this product?
            if (product.Id != updatecartitem.ProductId)
                return LocalRedirect(productUrl);
        }

        //display "edit" (manage) link
        if (await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) &&
            await _permissionService.AuthorizeAsync(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE))
        {
            //a vendor should have access only to his products
            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor == null || currentVendor.Id == product.VendorId)
            {
               // DisplayEditLink(Url.Action("Edit", "Product", new { id = product.Id, area = AreaNames.ADMIN }));
            }
        }

        //activity log
        await _customerActivityService.InsertActivityAsync("PublicStore.ViewProduct",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ViewProduct"), product.Name), product);

        //model
        var model = await _productModelFactory.PrepareProductDetailsDtoAsync(product, updatecartitem);

        //template
        var productTemplateViewPath = await _productModelFactory.PrepareProductTemplateViewPathAsync(product);

        var response = new ProductDetailsResponse
        {
            ProductDetails = model,
            ProductTemplateViewPath = productTemplateViewPath
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("EstimateShipping", Name = "EstimateShipping")]
    [ProducesResponseType(typeof(EstimateShippingResultDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> EstimateShipping([FromQuery] ProductEstimateShippingDto model, IFormCollection form)
    {
        if (model == null)
            model = new ProductEstimateShippingDto();

        var errors = new List<string>();

        if (!_shippingSettings.EstimateShippingCityNameEnabled && string.IsNullOrEmpty(model.ZipPostalCode))
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShipping.ZipPostalCode.Required"));

        if (_shippingSettings.EstimateShippingCityNameEnabled && string.IsNullOrEmpty(model.City))
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShipping.City.Required"));

        if (model.CountryId == null || model.CountryId == 0)
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShipping.Country.Required"));

        if (errors.Count > 0)
            return Ok(new
            {
                Success = false,
                Errors = errors
            });

        var product = await _productService.GetProductByIdAsync(model.ProductId);
        if (product == null || product.Deleted)
        {
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShippingPopUp.Product.IsNotFound"));
            return Ok(new
            {
                Success = false,
                Errors = errors
            });
        }

        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();

        var wrappedProduct = new ShoppingCartItem()
        {
            StoreId = store.Id,
            CustomerId = customer.Id,
            ProductId = product.Id,
            CreatedOnUtc = DateTime.UtcNow
        };

        var addToCartWarnings = new List<string>();

        //entered quantity
        wrappedProduct.Quantity = _productAttributeParser.ParseEnteredQuantity(product, form);

        //product and gift card attributes
        wrappedProduct.AttributesXml = await _productAttributeParser.ParseProductAttributesAsync(product, form, addToCartWarnings);


        var result = await _shoppingCartModelFactory.PrepareEstimateShippingResultModelAsync(new[] { wrappedProduct }, model, false);

        return Ok(result);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("GetProductCombinations/{productId}", Name = "GetProductCombinations")]
    [ProducesResponseType(typeof(IList<ProductCombinationDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetProductCombinations([FromRoute] int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            return NotFound();

        var model = await _productModelFactory.PrepareProductCombinationDtosAsync(product);
        return Ok(model);
    }

    #endregion

    #region Product reviews

    [HttpGet]
    [Route("ProductReviews/{productId}", Name = "ProductReviews")]
    [ProducesResponseType(typeof(ProductReviewsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductReviews([FromRoute] int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null || product.Deleted || !product.Published )
            return Error();

        var model = new ProductReviewsDto();
        model = await _productModelFactory.PrepareProductReviewsModelAsync(product);

        await ValidateProductReviewAvailabilityAsync(product);

        //default value
        model.AddProductReview.Rating = _catalogSettings.DefaultProductRatingValue;

        //default value for all additional review types
        if (model.ReviewTypeList.Count > 0)
            foreach (var additionalProductReview in model.AddAdditionalProductReviewList)
            {
                additionalProductReview.Rating = additionalProductReview.IsRequired ? _catalogSettings.DefaultProductRatingValue : 0;
            }

        return Ok(model);
    }


    //[ValidateCaptcha]
    [HttpPost]
    [Route("ProductReviewsAdd/{productId}", Name = "ProductReviewsAdd")]
    [ProducesResponseType(typeof(ProductReviewsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductReviewsAdd([FromRoute] int productId, ProductReviewsDto model, bool captchaValid)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        var currentStore = await _storeContext.GetCurrentStoreAsync();

        if (product == null || product.Deleted || !product.Published || 
            !await _productReviewService.CanAddReviewAsync(product.Id, _catalogSettings.ShowProductReviewsPerStore ? currentStore.Id : 0))
            return Error();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnProductReviewPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        await ValidateProductReviewAvailabilityAsync(product);

        if (ModelState.IsValid)
        {
            //save review
            var rating = model.AddProductReview.Rating;
            if (rating < 1 || rating > 5)
                rating = _catalogSettings.DefaultProductRatingValue;
            var isApproved = !_catalogSettings.ProductReviewsMustBeApproved;
            var customer = await _workContext.GetCurrentCustomerAsync();

            var productReview = new ProductReview
            {
                ProductId = product.Id,
                CustomerId = customer.Id,
                Title = model.AddProductReview.Title,
                ReviewText = model.AddProductReview.ReviewText,
                Rating = rating,
                HelpfulYesTotal = 0,
                HelpfulNoTotal = 0,
                IsApproved = isApproved,
                CreatedOnUtc = DateTime.UtcNow,
                StoreId = currentStore.Id,
            };

            await _productReviewService.InsertProductReviewAsync(productReview);

            //add product review and review type mapping                
            foreach (var additionalReview in model.AddAdditionalProductReviewList)
            {
                var additionalProductReview = new ProductReviewReviewTypeMapping
                {
                    ProductReviewId = productReview.Id,
                    ReviewTypeId = additionalReview.ReviewTypeId,
                    Rating = additionalReview.Rating
                };

                await _reviewTypeService.InsertProductReviewReviewTypeMappingsAsync(additionalProductReview);
            }

            //update product totals
            await _productReviewService.UpdateProductReviewTotalsAsync(product);

            //notify store owner
            if (_catalogSettings.NotifyStoreOwnerAboutNewProductReviews)
                await _workflowMessageService.SendProductReviewStoreOwnerNotificationMessageAsync(productReview, _localizationSettings.DefaultAdminLanguageId);

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.AddProductReview",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddProductReview"), product.Name), product);

            //raise event
            if (productReview.IsApproved)
                await _eventPublisher.PublishAsync(new ProductReviewApprovedEvent(productReview));

            model = await _productModelFactory.PrepareProductReviewsModelAsync(product);
            model.AddProductReview.Title = null;
            model.AddProductReview.ReviewText = null;

            if (!isApproved)
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Reviews.SeeAfterApproving"));
            else
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Reviews.SuccessfullyAdded"));

            var seName = await _urlRecordService.GetSeNameAsync(product);
            var productUrl = await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = seName });
            return LocalRedirect(productUrl);
        }

        //If we got this far, something failed, redisplay form
        RouteData.Values["action"] = "ProductDetails";

        //model
        var productModel = await _productModelFactory.PrepareProductDetailsDtoAsync(product);
        ////template
        //var productTemplateViewPath = await _productModelFactory.PrepareProductTemplateViewPathAsync(product);

        return Ok(productModel);
    }


    [HttpPost]
    [Route("SetProductReviewHelpfulness/{productReviewId}", Name = "SetProductReviewHelpfulness")]
    [ProducesResponseType(typeof(SetProductReviewHelpfulnessResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> SetProductReviewHelpfulness([FromRoute] int productReviewId, [FromQuery] bool washelpful)
    {
        var productReview = await _productReviewService.GetProductReviewByIdAsync(productReviewId) ?? throw new ArgumentException("No product review found with the specified id");

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
        {
            return Ok(new
            {
                Result = await _localizationService.GetResourceAsync("Reviews.Helpfulness.OnlyRegistered"),
                TotalYes = productReview.HelpfulYesTotal,
                TotalNo = productReview.HelpfulNoTotal
            });
        }

        //customers aren't allowed to vote for their own reviews
        if (productReview.CustomerId == customer.Id)
        {
            return Ok(new
            {
                Result = await _localizationService.GetResourceAsync("Reviews.Helpfulness.YourOwnReview"),
                TotalYes = productReview.HelpfulYesTotal,
                TotalNo = productReview.HelpfulNoTotal
            });
        }

        await _productReviewService.SetProductReviewHelpfulnessAsync(productReview, washelpful);

        //new totals
        await _productReviewService.UpdateProductReviewHelpfulnessTotalsAsync(productReview);

        return Ok(new
        {
            Result = await _localizationService.GetResourceAsync("Reviews.Helpfulness.SuccessfullyVoted"),
            TotalYes = productReview.HelpfulYesTotal,
            TotalNo = productReview.HelpfulNoTotal
        });
    }

    [HttpGet]
    [Route("CustomerProductReviews", Name = "CustomerProductReviews")]
    [ProducesResponseType(typeof(DTOs.Catalog.CustomerProductReviewsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> CustomerProductReviews([FromQuery] int? pageNumber)
    {
        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        if (!_catalogSettings.ShowProductReviewsTabOnAccountPage)
        {
            return Error(); // RedirectToRoute("CustomerInfo");
        }

        var model = await _productModelFactory.PrepareCustomerProductReviewsModelAsync(pageNumber);

        return Ok(model);
    }

    #endregion

    #region Email a friend

    [HttpGet]
    [Route("ProductEmailAFriend/{productId}", Name = "ProductEmailAFriend")]
    [ProducesResponseType(typeof(ProductEmailAFriendDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductEmailAFriend([FromRoute] int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null || product.Deleted || !product.Published || !_catalogSettings.EmailAFriendEnabled)
            return Error();

        var model = new ProductEmailAFriendDto();
        model = await _productModelFactory.PrepareProductEmailAFriendDtoAsync(model, product, false);
        return Ok(model);
    }


    [FormValueRequired("send-email")]
    //[ValidateCaptcha]
    [HttpPost]
    [Route("ProductEmailAFriendSend", Name = "ProductEmailAFriendSend")]
    [ProducesResponseType(typeof(ProductEmailAFriendDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductEmailAFriendSend(ProductEmailAFriendDto model, bool captchaValid)
    {
        var product = await _productService.GetProductByIdAsync(model.ProductId);
        if (product == null || product.Deleted || !product.Published || !_catalogSettings.EmailAFriendEnabled)
            return Error();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnEmailProductToFriendPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        //check whether the current customer is guest and ia allowed to email a friend
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !_catalogSettings.AllowAnonymousUsersToEmailAFriend)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Products.EmailAFriend.OnlyRegisteredUsers"));
        }

        if (ModelState.IsValid)
        {
            //email
            await _workflowMessageService.SendProductEmailAFriendMessageAsync(customer,
                (await _workContext.GetWorkingLanguageAsync()).Id, product,
                model.YourEmailAddress, model.FriendEmail,
                _htmlFormatter.FormatText(model.PersonalMessage, false, true, false, false, false, false));

            model = await _productModelFactory.PrepareProductEmailAFriendDtoAsync(model, product, true);
            model.SuccessfullySent = true;
            model.Result = await _localizationService.GetResourceAsync("Products.EmailAFriend.SuccessfullySent");

            return Ok(model);
        }

        //If we got this far, something failed, redisplay form
        model = await _productModelFactory.PrepareProductEmailAFriendDtoAsync(model, product, true);
        return Ok(model);
    }

    #endregion

    #region Components


    [HttpGet]
    [Route("GetProductsAlsoPurchased", Name = "GetProductsAlsoPurchased")]
    [ProducesResponseType(typeof(IList<ProductOverviewDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductsAlsoPurchased(int productId, int? productThumbPictureSize)
    {
        if (!_catalogSettings.ProductsAlsoPurchasedEnabled)
            return Content("");

        //load and cache report
        var store = await _storeContext.GetCurrentStoreAsync();
        var productIds = await _staticCacheManager.GetAsync(_staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsKey, productId, store),
            async () => await _orderReportService.GetAlsoPurchasedProductsIdsAsync(store.Id, productId, _catalogSettings.ProductsAlsoPurchasedNumber)
        );

        //load products
        var products = await (await _productService.GetProductsByIdsAsync(productIds))
            //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();

        if (!products.Any())
            return Content("");

        var model = (await _productModelFactory.PrepareProductOverviewDtosAsync(products, true, true, productThumbPictureSize)).ToList();
        return Ok(model);
    }

    #endregion
}
