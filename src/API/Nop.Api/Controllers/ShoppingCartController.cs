using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Shipping;
using Nop.Core.Http.Extensions;
using Nop.Core.Infrastructure;
using Nop.Services.Attributes;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Api.Factories;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.Infrastructure.Cache;
using Nop.Api.DTOs.Media;
using Nop.Api.DTOs.ShoppingCart;
using System.Net;
using Nop.Api.DTOs.Responses;
using MailKit;
using Nop.Api.DTOs.Polls;

namespace Nop.Api.Controllers;

public partial class ShoppingCartController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CustomerSettings _customerSettings;
    protected readonly IAttributeParser<CheckoutAttribute, CheckoutAttributeValue> _checkoutAttributeParser;
    protected readonly IAttributeService<CheckoutAttribute, CheckoutAttributeValue> _checkoutAttributeService;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IDiscountService _discountService;
    protected readonly IDownloadService _downloadService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IGiftCardService _giftCardService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILocalizationService _localizationService;
    protected readonly INopFileProvider _fileProvider;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly IProductService _productService;
    protected readonly IShippingService _shippingService;
    protected readonly IShoppingCartDtoFactory _shoppingCartModelFactory;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly ITaxService _taxService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly MediaSettings _mediaSettings;
    protected readonly OrderSettings _orderSettings;
    protected readonly ShoppingCartSettings _shoppingCartSettings;
    protected readonly ShippingSettings _shippingSettings;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public ShoppingCartController(CaptchaSettings captchaSettings,
        CustomerSettings customerSettings,
        IAttributeParser<CheckoutAttribute, CheckoutAttributeValue> checkoutAttributeParser,
        IAttributeService<CheckoutAttribute, CheckoutAttributeValue> checkoutAttributeService,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDiscountService discountService,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IGiftCardService giftCardService,
        IHtmlFormatter htmlFormatter,
        ILocalizationService localizationService,
        INopFileProvider fileProvider,
        INopUrlHelper nopUrlHelper,
        INotificationService notificationService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceFormatter priceFormatter,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IProductService productService,
        IShippingService shippingService,
        IShoppingCartDtoFactory shoppingCartModelFactory,
        IShoppingCartService shoppingCartService,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        ITaxService taxService,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        MediaSettings mediaSettings,
        OrderSettings orderSettings,
        ShoppingCartSettings shoppingCartSettings,
        ShippingSettings shippingSettings)
    {
        _captchaSettings = captchaSettings;
        _customerSettings = customerSettings;
        _checkoutAttributeParser = checkoutAttributeParser;
        _checkoutAttributeService = checkoutAttributeService;
        _currencyService = currencyService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _discountService = discountService;
        _downloadService = downloadService;
        _genericAttributeService = genericAttributeService;
        _giftCardService = giftCardService;
        _htmlFormatter = htmlFormatter;
        _localizationService = localizationService;
        _fileProvider = fileProvider;
        _nopUrlHelper = nopUrlHelper;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _priceFormatter = priceFormatter;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _productService = productService;
        _shippingService = shippingService;
        _shoppingCartModelFactory = shoppingCartModelFactory;
        _shoppingCartService = shoppingCartService;
        _staticCacheManager = staticCacheManager;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _taxService = taxService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _mediaSettings = mediaSettings;
        _orderSettings = orderSettings;
        _shoppingCartSettings = shoppingCartSettings;
        _shippingSettings = shippingSettings;
    }

    #endregion

    #region Utilities

    protected virtual async Task ParseAndSaveCheckoutAttributesAsync(IList<ShoppingCartItem> cart, IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(cart);

        ArgumentNullException.ThrowIfNull(form);

        var attributesXml = string.Empty;
        var excludeShippableAttributes = !await _shoppingCartService.ShoppingCartRequiresShippingAsync(cart);
        var store = await _storeContext.GetCurrentStoreAsync();
        var checkoutAttributes = await _checkoutAttributeService.GetAllAttributesAsync(_staticCacheManager, _storeMappingService, store.Id, excludeShippableAttributes);
        foreach (var attribute in checkoutAttributes)
        {
            var controlId = $"checkout_attribute_{attribute.Id}";
            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                {
                    var ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var selectedAttributeId = int.Parse(ctrlAttributes);
                        if (selectedAttributeId > 0)
                            attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                    }
                }

                    break;
                case AttributeControlType.Checkboxes:
                {
                    var cblAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(cblAttributes))
                    {
                        foreach (var item in cblAttributes.ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var selectedAttributeId = int.Parse(item);
                            if (selectedAttributeId > 0)
                                attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }
                }

                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                {
                    //load read-only (already server-side selected) values
                    var attributeValues = await _checkoutAttributeService.GetAttributeValuesAsync(attribute.Id);
                    foreach (var selectedAttributeId in attributeValues
                                 .Where(v => v.IsPreSelected)
                                 .Select(v => v.Id)
                                 .ToList())
                    {
                        attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                            attribute, selectedAttributeId.ToString());
                    }
                }

                    break;
                case AttributeControlType.TextBox:
                case AttributeControlType.MultilineTextbox:
                {
                    var ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var enteredText = ctrlAttributes.ToString().Trim();
                        attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                            attribute, enteredText);
                    }
                }

                    break;
                case AttributeControlType.Datepicker:
                {
                    var date = form[controlId + "_day"];
                    var month = form[controlId + "_month"];
                    var year = form[controlId + "_year"];
                    DateTime? selectedDate = null;
                    try
                    {
                        selectedDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(date));
                    }
                    catch
                    {
                        // ignored
                    }

                    if (selectedDate.HasValue)
                        attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                            attribute, selectedDate.Value.ToString("D"));
                }

                    break;
                case AttributeControlType.FileUpload:
                {
                    _ = Guid.TryParse(form[controlId], out var downloadGuid);
                    var download = await _downloadService.GetDownloadByGuidAsync(downloadGuid);
                    if (download != null)
                    {
                        attributesXml = _checkoutAttributeParser.AddAttribute(attributesXml,
                            attribute, download.DownloadGuid.ToString());
                    }
                }

                    break;
                default:
                    break;
            }
        }

        //validate conditional attributes (if specified)
        foreach (var attribute in checkoutAttributes)
        {
            var conditionMet = await _checkoutAttributeParser.IsConditionMetAsync(attribute.ConditionAttributeXml, attributesXml);
            if (conditionMet.HasValue && !conditionMet.Value)
                attributesXml = _checkoutAttributeParser.RemoveAttribute(attributesXml, attribute.Id);
        }

        //save checkout attributes
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(), NopCustomerDefaults.CheckoutAttributes, attributesXml, store.Id);
    }

    protected virtual async Task SaveItemAsync(ShoppingCartItem updatecartitem, List<string> addToCartWarnings, Product product,
        ShoppingCartType cartType, string attributes,  int quantity)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        if (updatecartitem == null)
        {
            //add to the cart
            addToCartWarnings.AddRange(await _shoppingCartService.AddToCartAsync(customer,
                product, cartType, store.Id,
                attributes, quantity, true));
        }
        else
        {
            var cart = await _shoppingCartService.GetShoppingCartAsync(customer, updatecartitem.ShoppingCartType, store.Id);

            var otherCartItemWithSameParameters = await _shoppingCartService.FindShoppingCartItemInTheCartAsync(
                cart, updatecartitem.ShoppingCartType, product, attributes);
            if (otherCartItemWithSameParameters != null &&
                otherCartItemWithSameParameters.Id == updatecartitem.Id)
            {
                //ensure it's some other shopping cart item
                otherCartItemWithSameParameters = null;
            }
            //update existing item
            addToCartWarnings.AddRange(await _shoppingCartService.UpdateShoppingCartItemAsync(customer,
                updatecartitem.Id, attributes,quantity + (otherCartItemWithSameParameters?.Quantity ?? 0), true));
            if (otherCartItemWithSameParameters != null && !addToCartWarnings.Any())
            {
                //delete the same shopping cart item (the other one)
                await _shoppingCartService.DeleteShoppingCartItemAsync(otherCartItemWithSameParameters);
            }
        }
    }

    protected virtual async Task<IActionResult> GetProductToCartDetailsAsync(List<string> addToCartWarnings, ShoppingCartType cartType,
        Product product)
    {
        if (addToCartWarnings.Any())
        {
            //cannot be added to the cart/wishlist
            //let's display warnings
            return Ok(new
            {
                success = false,
                message = addToCartWarnings.ToArray()
            });
        }

        //added to the cart/wishlist
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        switch (cartType)
        {
            case ShoppingCartType.Wishlist:
            {
                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.AddToWishlist",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddToWishlist"), product.Name), product);

                if (_shoppingCartSettings.DisplayWishlistAfterAddingProduct)
                {
                    //redirect to the wishlist page
                    return Ok(new
                    {
                        redirect = Url.RouteUrl("Wishlist")
                    });
                }

                //display notification message and update appropriate blocks
                var shoppingCarts = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);

                var updateTopWishlistSectionHtml = string.Format(
                    await _localizationService.GetResourceAsync("Wishlist.HeaderQuantity"),
                    shoppingCarts.Sum(item => item.Quantity));

                return Ok(new
                {
                    success = true,
                    message = string.Format(
                        await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheWishlist.Link"),
                        Url.RouteUrl("Wishlist")),
                    updatetopwishlistsectionhtml = updateTopWishlistSectionHtml
                });
            }

            case ShoppingCartType.ShoppingCart:
            default:
            {
                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.AddToShoppingCart",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddToShoppingCart"), product.Name), product);

                if (_shoppingCartSettings.DisplayCartAfterAddingProduct)
                {
                    //redirect to the shopping cart page
                    return Ok(new
                    {
                        redirect = Url.RouteUrl("ShoppingCart")
                    });
                }

                //display notification message and update appropriate blocks
                var shoppingCarts = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

                var updateTopCartSectionHtml = string.Format(
                    await _localizationService.GetResourceAsync("ShoppingCart.HeaderQuantity"),
                    shoppingCarts.Sum(item => item.Quantity));

                //var updateFlyoutCartSectionHtml = _shoppingCartSettings.MiniShoppingCartEnabled
                //    ? await RenderViewComponentToStringAsync(typeof(FlyoutShoppingCartViewComponent))
                //    : string.Empty;

                return Ok(new
                {
                    success = true,
                    message = string.Format(await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheCart.Link"),
                        Url.RouteUrl("ShoppingCart")),
                    updatetopcartsectionhtml = updateTopCartSectionHtml,
                    //updateflyoutcartsectionhtml = updateFlyoutCartSectionHtml
                });
            }
        }
    }

    #endregion

    #region Shopping cart


    [HttpPost]
    [Route("SelectShippingOption", Name = "SelectShippingOption")]
    [ProducesResponseType(typeof(SelectShippingOptionResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SelectShippingOption([FromQuery] string name,  EstimateShippingDto model, IFormCollection form)
    {
        if (model == null)
            model = new EstimateShippingDto();

        var errors = new List<string>();
        if (string.IsNullOrEmpty(model.ZipPostalCode) && !_shippingSettings.EstimateShippingCityNameEnabled)
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShipping.ZipPostalCode.Required"));

        if (model.CountryId == null || model.CountryId == 0)
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShipping.Country.Required"));

        if (errors.Count > 0)
            return Ok(new
            {
                success = false,
                errors = errors
            });

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);
        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        var shippingOptions = new List<ShippingOption>();
        ShippingOption selectedShippingOption = null;

        if (!string.IsNullOrWhiteSpace(name))
        {
            //find shipping options
            //performance optimization. try cache first
            shippingOptions = await _genericAttributeService.GetAttributeAsync<List<ShippingOption>>(customer,
                NopCustomerDefaults.OfferedShippingOptionsAttribute, store.Id);

            if (shippingOptions == null || !shippingOptions.Any())
            {
                var address = new Address
                {
                    CountryId = model.CountryId,
                    StateProvinceId = model.StateProvinceId,
                    ZipPostalCode = model.ZipPostalCode,
                };

                //not found? let's load them using shipping service
                var getShippingOptionResponse = await _shippingService.GetShippingOptionsAsync(cart, address,
                    customer, storeId: store.Id);

                if (getShippingOptionResponse.Success)
                    shippingOptions = getShippingOptionResponse.ShippingOptions.ToList();
                else
                    foreach (var error in getShippingOptionResponse.Errors)
                        errors.Add(error);
            }
        }

        selectedShippingOption = shippingOptions.Find(so => !string.IsNullOrEmpty(so.Name) && so.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        if (selectedShippingOption == null)
            errors.Add(await _localizationService.GetResourceAsync("Shipping.EstimateShippingPopUp.ShippingOption.IsNotFound"));

        if (errors.Count > 0)
            return Ok(new
            {
                success = false,
                errors = errors
            });

        //reset pickup point
        await _genericAttributeService.SaveAttributeAsync<PickupPoint>(customer,
            NopCustomerDefaults.SelectedPickupPointAttribute, null, store.Id);

        //cache shipping option
        await _genericAttributeService.SaveAttributeAsync(customer,
            NopCustomerDefaults.SelectedShippingOptionAttribute, selectedShippingOption, store.Id);

        //var orderTotalsSectionHtml = await RenderViewComponentToStringAsync(typeof(OrderTotalsViewComponent), new { isEditable = true });

        return Ok(new
        {
            success = true,
            //ordertotalssectionhtml = orderTotalsSectionHtml
        });
    }

    //add product to cart using AJAX
    //currently we use this method on catalog pages (category/manufacturer/etc)

    [HttpPost]
    [Route("AddProductToCartFromCatalog/{productId}", Name = "AddProductToCartFromCatalog")]
    [ProducesResponseType(typeof(AddProductToCartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddProductToCart_Catalog([FromRoute] int productId, [FromQuery] int shoppingCartTypeId,
       [FromQuery] int quantity, bool forceredirection = false)
    {
        var cartType = (ShoppingCartType)shoppingCartTypeId;

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            //no product found
            return Ok(new
            {
                success = false,
                message = "No product found with the specified ID"
            });

        var redirectUrl = await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = await _urlRecordService.GetSeNameAsync(product) });

        //we can add only simple products
        if (product.ProductType != ProductType.SimpleProduct)
            return Ok(new { redirect = redirectUrl });

        //products with "minimum order quantity" more than a specified qty
        if (product.OrderMinimumQuantity > quantity)
        {
            //we cannot add to the cart such products from category pages
            //it can confuse customers. That's why we redirect customers to the product details page
            return Ok(new { redirect = redirectUrl });
        }

        var allowedQuantities = _productService.ParseAllowedQuantities(product);
        if (allowedQuantities.Length > 0)
        {
            //cannot be added to the cart (requires a customer to select a quantity from dropdownlist)
            return Ok(new { redirect = redirectUrl });
        }

        //allow a product to be added to the cart when all attributes are with "read-only checkboxes" type
        var productAttributes = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(product.Id);
        if (productAttributes.Any(pam => pam.AttributeControlType != AttributeControlType.ReadonlyCheckboxes))
        {
            //product has some attributes. let a customer see them
            return Ok(new { redirect = redirectUrl });
        }

        //creating XML for "read-only checkboxes" attributes
        var attXml = await productAttributes.AggregateAwaitAsync(string.Empty, async (attributesXml, attribute) =>
        {
            var attributeValues = await _productAttributeService.GetProductAttributeValuesAsync(attribute.Id);
            foreach (var selectedAttributeId in attributeValues
                         .Where(v => v.IsPreSelected)
                         .Select(v => v.Id)
                         .ToList())
            {
                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                    attribute, selectedAttributeId.ToString());
            }

            return attributesXml;
        });

        //get standard warnings without attribute validations
        //first, try to find existing shopping cart item
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, cartType, store.Id);
        var shoppingCartItem = await _shoppingCartService.FindShoppingCartItemInTheCartAsync(cart, cartType, product);
        //if we already have the same product in the cart, then use the total quantity to validate
        var quantityToValidate = shoppingCartItem != null ? shoppingCartItem.Quantity + quantity : quantity;
        var addToCartWarnings = await _shoppingCartService
            .GetShoppingCartItemWarningsAsync(customer, cartType,
                product, store.Id, string.Empty,
                quantityToValidate, false, shoppingCartItem?.Id ?? 0, true, false, false, false);
        if (addToCartWarnings.Any())
        {
            //cannot be added to the cart
            //let's display standard warnings
            return Ok(new
            {
                success = false,
                message = addToCartWarnings.ToArray()
            });
        }

        //now let's try adding product to the cart (now including product attribute validation, etc)
        addToCartWarnings = await _shoppingCartService.AddToCartAsync(customer: customer,
            product: product,
            shoppingCartType: cartType,
            storeId: store.Id,
            attributesXml: attXml,
            quantity: quantity);
        if (addToCartWarnings.Any())
        {
            //cannot be added to the cart
            //but we do not display attribute and gift card warnings here. let's do it on the product details page
            return Ok(new { redirect = redirectUrl });
        }

        //added to the cart/wishlist
        switch (cartType)
        {
            case ShoppingCartType.Wishlist:
            {
                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.AddToWishlist",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddToWishlist"), product.Name), product);

                if (_shoppingCartSettings.DisplayWishlistAfterAddingProduct || forceredirection)
                {
                    //redirect to the wishlist page
                    return Ok(new
                    {
                        redirect = Url.RouteUrl("Wishlist")
                    });
                }

                //display notification message and update appropriate blocks
                var shoppingCarts = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);

                var updatetopwishlistsectionhtml = string.Format(await _localizationService.GetResourceAsync("Wishlist.HeaderQuantity"),
                    shoppingCarts.Sum(item => item.Quantity));
                return Ok(new
                {
                    success = true,
                    message = string.Format(await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheWishlist.Link"), Url.RouteUrl("Wishlist")),
                    updatetopwishlistsectionhtml
                });
            }

            case ShoppingCartType.ShoppingCart:
            default:
            {
                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.AddToShoppingCart",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddToShoppingCart"), product.Name), product);

                if (_shoppingCartSettings.DisplayCartAfterAddingProduct || forceredirection)
                {
                    //redirect to the shopping cart page
                    return Ok(new
                    {
                        redirect = Url.RouteUrl("ShoppingCart")
                    });
                }

                //display notification message and update appropriate blocks
                var shoppingCarts = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

                var updatetopcartsectionhtml = string.Format(await _localizationService.GetResourceAsync("ShoppingCart.HeaderQuantity"),
                    shoppingCarts.Sum(item => item.Quantity));

                //var updateflyoutcartsectionhtml = _shoppingCartSettings.MiniShoppingCartEnabled
                //    ? await RenderViewComponentToStringAsync(typeof(FlyoutShoppingCartViewComponent))
                //    : string.Empty;

                return Ok(new
                {
                    success = true,
                    message = string.Format(await _localizationService.GetResourceAsync("Products.ProductHasBeenAddedToTheCart.Link"), Url.RouteUrl("ShoppingCart")),
                    updatetopcartsectionhtml,
                    //updateflyoutcartsectionhtml
                });
            }
        }
    }

    //add product to cart using AJAX
    //currently we use this method on the product details pages

    [HttpPost]
    [Route("AddProductToCartFromDetails/{productId}", Name = "AddProductToCartFromDetails")]
    [ProducesResponseType(typeof(AddProductToCartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddProductToCart_Details([FromRoute] int productId, [FromQuery] int shoppingCartTypeId, IFormCollection form)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
        {
            return Ok(new
            {
                redirect = Url.RouteUrl("Homepage")
            });
        }

        //we can add only simple products
        if (product.ProductType != ProductType.SimpleProduct)
        {
            return Ok(new
            {
                success = false,
                message = "Only simple products could be added to the cart"
            });
        }

        //update existing shopping cart item
        var updatecartitemid = 0;
        foreach (var formKey in form.Keys)
            if (formKey.Equals($"addtocart_{productId}.UpdatedShoppingCartItemId", StringComparison.InvariantCultureIgnoreCase))
            {
                _ = int.TryParse(form[formKey], out updatecartitemid);
                break;
            }

        ShoppingCartItem updatecartitem = null;
        if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            //search with the same cart type as specified
            var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), (ShoppingCartType)shoppingCartTypeId, store.Id);

            updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
            //not found? let's ignore it. in this case we'll add a new item
            //if (updatecartitem == null)
            //{
            //    return Ok(new
            //    {
            //        success = false,
            //        message = "No shopping cart item found to update"
            //    });
            //}
            //is it this product?
            if (updatecartitem != null && product.Id != updatecartitem.ProductId)
            {
                return Ok(new
                {
                    success = false,
                    message = "This product does not match a passed shopping cart item identifier"
                });
            }
        }

        var addToCartWarnings = new List<string>();


        //entered quantity
        var quantity = _productAttributeParser.ParseEnteredQuantity(product, form);

        //product and gift card attributes
        var attributes = await _productAttributeParser.ParseProductAttributesAsync(product, form, addToCartWarnings);


        var cartType = updatecartitem == null ? (ShoppingCartType)shoppingCartTypeId :
            //if the item to update is found, then we ignore the specified "shoppingCartTypeId" parameter
            updatecartitem.ShoppingCartType;

        await SaveItemAsync(updatecartitem, addToCartWarnings, product, cartType, attributes, quantity);

        //return result
        return await GetProductToCartDetailsAsync(addToCartWarnings, cartType, product);
    }

    //handle product attribute selection event. this way we return new price, overridden gtin/sku/mpn
    //currently we use this method on the product details pages

    [HttpPut]
    [Route("ProductDetailsAttributeChange/{productId}", Name = "ProductDetailsAttributeChange")]
    [ProducesResponseType(typeof(ProductDetailsAttributeChangeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ProductDetails_AttributeChange([FromRoute] int productId, [FromQuery] bool validateAttributeConditions,
        [FromQuery] bool loadPicture, IFormCollection form)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            return new NullJsonResult();

        var errors = new List<string>();
        var attributeXml = await _productAttributeParser.ParseProductAttributesAsync(product, form, errors);

        //sku, mpn, gtin
        var sku = await _productService.FormatSkuAsync(product, attributeXml);
        var mpn = await _productService.FormatMpnAsync(product, attributeXml);
        var gtin = await _productService.FormatGtinAsync(product, attributeXml);

        // calculating weight adjustment
        var attributeValues = await _productAttributeParser.ParseProductAttributeValuesAsync(attributeXml);
        var totalWeight = product.BasepriceAmount;

        foreach (var attributeValue in attributeValues)
        {
            switch (attributeValue.AttributeValueType)
            {
                case AttributeValueType.Simple:
                    //simple attribute
                    totalWeight += attributeValue.WeightAdjustment;
                    break;
                case AttributeValueType.AssociatedToProduct:
                    //bundled product
                    var associatedProduct = await _productService.GetProductByIdAsync(attributeValue.AssociatedProductId);
                    if (associatedProduct != null)
                        totalWeight += associatedProduct.BasepriceAmount * attributeValue.Quantity;
                    break;
            }
        }

        //price
        var price = string.Empty;
        //base price
        var basepricepangv = string.Empty;
        if (await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.DISPLAY_PRICES))
        {
            var currentStore = await _storeContext.GetCurrentStoreAsync();
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();

            //we do not calculate price of "customer enters price" option is enabled
            var (finalPrice, _, _) = await _shoppingCartService.GetUnitPriceAsync(product,
                currentCustomer,
                currentStore,
                ShoppingCartType.ShoppingCart,
                1, attributeXml, true);
            var (finalPriceWithDiscountBase, _) = await _taxService.GetProductPriceAsync(product, finalPrice);
            var finalPriceWithDiscount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(finalPriceWithDiscountBase, await _workContext.GetWorkingCurrencyAsync());
            price = await _priceFormatter.FormatPriceAsync(finalPriceWithDiscount);
            basepricepangv = await _priceFormatter.FormatBasePriceAsync(product, finalPriceWithDiscountBase, totalWeight);
        }

        //stock
        var stockAvailability = await _productService.FormatStockMessageAsync(product, attributeXml);

        //conditional attributes
        var enabledAttributeMappingIds = new List<int>();
        var disabledAttributeMappingIds = new List<int>();
        if (validateAttributeConditions)
        {
            var attributes = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(product.Id);
            foreach (var attribute in attributes)
            {
                var conditionMet = await _productAttributeParser.IsConditionMetAsync(attribute, attributeXml);
                if (conditionMet.HasValue)
                {
                    if (conditionMet.Value)
                        enabledAttributeMappingIds.Add(attribute.Id);
                    else
                        disabledAttributeMappingIds.Add(attribute.Id);
                }
            }
        }

        //picture. used when we want to override a default product picture when some attribute is selected
        var pictureFullSizeUrl = string.Empty;
        var pictureDefaultSizeUrl = string.Empty;
        var pictureIds = new List<int>();
        if (loadPicture)
        {
            //first, try to get product attribute combination picture
            var pictureId = 0;
            var combination = await _productAttributeParser.FindProductAttributeCombinationAsync(product, attributeXml);
            if (combination != null)
            {
                var combinationPictures = await _productAttributeService.GetProductAttributeCombinationPicturesAsync(combination.Id);
                pictureIds = combinationPictures.Select(cp => cp.PictureId).ToList();
                pictureId = combinationPictures.FirstOrDefault()?.PictureId ?? 0;
            }

            //then, let's see whether we have attribute values with pictures
            if (pictureId == 0)
            {
                var valuePictures = await (await _productAttributeParser.ParseProductAttributeValuesAsync(attributeXml))
                    .SelectManyAwait(async attributeValue => await _productAttributeService.GetProductAttributeValuePicturesAsync(attributeValue.Id))
                    .ToListAsync();
                pictureIds = valuePictures.Select(vp => vp.PictureId).ToList();
                pictureId = valuePictures.FirstOrDefault()?.PictureId ?? 0;
            }

            if (pictureId > 0)
            {
                var productAttributePictureCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.ProductAttributePictureDtoKey,
                    pictureId, _webHelper.IsCurrentConnectionSecured(), await _storeContext.GetCurrentStoreAsync());
                var pictureModel = await _staticCacheManager.GetAsync(productAttributePictureCacheKey, async () =>
                {
                    var picture = await _pictureService.GetPictureByIdAsync(pictureId);
                    string fullSizeImageUrl, imageUrl;

                    (fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                    (imageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture, _mediaSettings.ProductDetailsPictureSize);

                    return picture == null ? new PictureDto() : new PictureDto
                    {
                        FullSizeImageUrl = fullSizeImageUrl,
                        ImageUrl = imageUrl
                    };
                });
                pictureFullSizeUrl = pictureModel.FullSizeImageUrl;
                pictureDefaultSizeUrl = pictureModel.ImageUrl;
            }
        }

        var isFreeShipping = product.IsFreeShipping;
        if (isFreeShipping && !string.IsNullOrEmpty(attributeXml))
        {
            isFreeShipping = await (await _productAttributeParser.ParseProductAttributeValuesAsync(attributeXml))
                .Where(attributeValue => attributeValue.AttributeValueType == AttributeValueType.AssociatedToProduct)
                .SelectAwait(async attributeValue => await _productService.GetProductByIdAsync(attributeValue.AssociatedProductId))
                .AllAsync(associatedProduct => associatedProduct == null || !associatedProduct.IsShipEnabled || associatedProduct.IsFreeShipping);
        }

        return Ok(new
        {
            productId,
            gtin,
            mpn,
            sku,
            price,
            basepricepangv,
            stockAvailability,
            enabledattributemappingids = enabledAttributeMappingIds.ToArray(),
            disabledattributemappingids = disabledAttributeMappingIds.ToArray(),
            pictureFullSizeUrl,
            pictureDefaultSizeUrl,
            pictureIds,
            isFreeShipping,
            message = errors.Any() ? errors.ToArray() : null
        });
    }

    [HttpPost]
    [Route("CheckoutAttributeChange", Name = "CheckoutAttributeChange")]
    [ProducesResponseType(typeof(CheckoutAttributeChangeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> CheckoutAttributeChange(IFormCollection form, bool isEditable)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //save selected attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);
        var attributeXml = await _genericAttributeService.GetAttributeAsync<string>(customer,
            NopCustomerDefaults.CheckoutAttributes, store.Id);

        //conditions
        var enabledAttributeIds = new List<int>();
        var disabledAttributeIds = new List<int>();
        var excludeShippableAttributes = !await _shoppingCartService.ShoppingCartRequiresShippingAsync(cart);
        var attributes = await _checkoutAttributeService.GetAllAttributesAsync(_staticCacheManager, _storeMappingService, store.Id, excludeShippableAttributes);
        foreach (var attribute in attributes)
        {
            var conditionMet = await _checkoutAttributeParser.IsConditionMetAsync(attribute.ConditionAttributeXml, attributeXml);
            if (conditionMet.HasValue)
            {
                if (conditionMet.Value)
                    enabledAttributeIds.Add(attribute.Id);
                else
                    disabledAttributeIds.Add(attribute.Id);
            }
        }

        //update blocks
        //var ordetotalssectionhtml = await RenderViewComponentToStringAsync(typeof(OrderTotalsViewComponent), new { isEditable });
        //var selectedcheckoutattributesssectionhtml = await RenderViewComponentToStringAsync(typeof(SelectedCheckoutAttributesViewComponent));

        return Ok(new
        {
            //ordetotalssectionhtml,
            //selectedcheckoutattributesssectionhtml,
            enabledattributeids = enabledAttributeIds.ToArray(),
            disabledattributeids = disabledAttributeIds.ToArray()
        });
    }


    [IgnoreAntiforgeryToken]

    [HttpPost]
    [Route("UploadFileProductAttribute/{attributeId}", Name = "UploadFileProductAttribute")]
    [ProducesResponseType(typeof(UploadFileProductAttributeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> UploadFileProductAttribute([FromRoute] int attributeId)
    {
        var attribute = await _productAttributeService.GetProductAttributeMappingByIdAsync(attributeId);
        if (attribute == null || attribute.AttributeControlType != AttributeControlType.FileUpload)
        {
            return Ok(new
            {
                success = false,
                downloadGuid = Guid.Empty
            });
        }

        var httpPostedFile = await Request.GetFirstOrDefaultFileAsync();
        if (httpPostedFile == null)
        {
            return Ok(new
            {
                success = false,
                message = "No file uploaded",
                downloadGuid = Guid.Empty
            });
        }

        var fileBinary = await _downloadService.GetDownloadBitsAsync(httpPostedFile);

        var qqFileNameParameter = "qqfilename";
        var fileName = httpPostedFile.FileName;
        if (string.IsNullOrEmpty(fileName) && await Request.IsFormKeyExistsAsync(qqFileNameParameter))
            fileName = await Request.GetFormValueAsync(qqFileNameParameter);
        //remove path (passed in IE)
        fileName = _fileProvider.GetFileName(fileName);

        var contentType = httpPostedFile.ContentType;

        var fileExtension = _fileProvider.GetFileExtension(fileName);
        if (!string.IsNullOrEmpty(fileExtension))
            fileExtension = fileExtension.ToLowerInvariant();

        if (attribute.ValidationFileMaximumSize.HasValue)
        {
            //compare in bytes
            var maxFileSizeBytes = attribute.ValidationFileMaximumSize.Value * 1024;
            if (fileBinary.Length > maxFileSizeBytes)
            {
                //when returning JSON the mime-type must be set to text/plain
                //otherwise some browsers will pop-up a "Save As" dialog.
                return Ok(new
                {
                    success = false,
                    message = string.Format(await _localizationService.GetResourceAsync("ShoppingCart.MaximumUploadedFileSize"), attribute.ValidationFileMaximumSize.Value),
                    downloadGuid = Guid.Empty
                });
            }
        }

        var download = new Download
        {
            DownloadGuid = Guid.NewGuid(),
            UseDownloadUrl = false,
            DownloadUrl = string.Empty,
            DownloadBinary = fileBinary,
            ContentType = contentType,
            //we store filename without extension for downloads
            Filename = _fileProvider.GetFileNameWithoutExtension(fileName),
            Extension = fileExtension,
            IsNew = true
        };
        await _downloadService.InsertDownloadAsync(download);

        //when returning JSON the mime-type must be set to text/plain
        //otherwise some browsers will pop-up a "Save As" dialog.
        return Ok(new
        {
            success = true,
            message = await _localizationService.GetResourceAsync("ShoppingCart.FileUploaded"),
            downloadUrl = Url.RouteUrl("DownloadGetFileUpload", new { downloadId = download.DownloadGuid }),
            downloadGuid = download.DownloadGuid
        });
    }


    [IgnoreAntiforgeryToken]
    [HttpPost]
    [Route("UploadFileCheckoutAttribute/{attributeId}", Name = "UploadFileCheckoutAttribute")]
    [ProducesResponseType(typeof(UploadFileCheckoutAttributeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> UploadFileCheckoutAttribute([FromRoute] int attributeId)
    {
        var attribute = await _checkoutAttributeService.GetAttributeByIdAsync(attributeId);
        if (attribute == null || attribute.AttributeControlType != AttributeControlType.FileUpload)
        {
            return Ok(new
            {
                success = false,
                downloadGuid = Guid.Empty
            });
        }

        var httpPostedFile = await Request.GetFirstOrDefaultFileAsync();
        if (httpPostedFile == null)
        {
            return Ok(new
            {
                success = false,
                message = "No file uploaded",
                downloadGuid = Guid.Empty
            });
        }

        var fileBinary = await _downloadService.GetDownloadBitsAsync(httpPostedFile);

        var qqFileNameParameter = "qqfilename";
        var fileName = httpPostedFile.FileName;
        if (string.IsNullOrEmpty(fileName) && await Request.IsFormKeyExistsAsync(qqFileNameParameter))
            fileName = await Request.GetFormValueAsync(qqFileNameParameter);
        //remove path (passed in IE)
        fileName = _fileProvider.GetFileName(fileName);

        var contentType = httpPostedFile.ContentType;

        var fileExtension = _fileProvider.GetFileExtension(fileName);
        if (!string.IsNullOrEmpty(fileExtension))
            fileExtension = fileExtension.ToLowerInvariant();

        if (attribute.ValidationFileMaximumSize.HasValue)
        {
            //compare in bytes
            var maxFileSizeBytes = attribute.ValidationFileMaximumSize.Value * 1024;
            if (fileBinary.Length > maxFileSizeBytes)
            {
                //when returning JSON the mime-type must be set to text/plain
                //otherwise some browsers will pop-up a "Save As" dialog.
                return Ok(new
                {
                    success = false,
                    message = string.Format(await _localizationService.GetResourceAsync("ShoppingCart.MaximumUploadedFileSize"), attribute.ValidationFileMaximumSize.Value),
                    downloadGuid = Guid.Empty
                });
            }
        }

        var download = new Download
        {
            DownloadGuid = Guid.NewGuid(),
            UseDownloadUrl = false,
            DownloadUrl = string.Empty,
            DownloadBinary = fileBinary,
            ContentType = contentType,
            //we store filename without extension for downloads
            Filename = _fileProvider.GetFileNameWithoutExtension(fileName),
            Extension = fileExtension,
            IsNew = true
        };
        await _downloadService.InsertDownloadAsync(download);

        //when returning JSON the mime-type must be set to text/plain
        //otherwise some browsers will pop-up a "Save As" dialog.
        return Ok(new
        {
            success = true,
            message = await _localizationService.GetResourceAsync("ShoppingCart.FileUploaded"),
            downloadUrl = Url.RouteUrl("DownloadGetFileUpload", new { downloadId = download.DownloadGuid }),
            downloadGuid = download.DownloadGuid
        });
    }

    [HttpGet]
    [Route("Cart", Name = "Cart")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Cart()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART))
            return Unauthorized();

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
        var model = new ShoppingCartDto();
        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);
        return Ok(model);
    }


    [FormValueRequired("updatecart")]

    [HttpPost]
    [Route("UpdateCart", Name = "UpdateCart")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> UpdateCart(IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART))
            return Unauthorized();

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //get identifiers of items to remove
        var itemIdsToRemove = form["removefromcart"]
            .SelectMany(value => value.Split(_separator, StringSplitOptions.RemoveEmptyEntries))
            .Select(idString => int.TryParse(idString, out var id) ? id : 0)
            .Distinct().ToList();

        var products = (await _productService.GetProductsByIdsAsync(cart.Select(item => item.ProductId).Distinct().ToArray()))
            .ToDictionary(item => item.Id, item => item);

        //get order items with changed quantity
        var itemsWithNewQuantity = cart.Select(item => new
        {
            //try to get a new quantity for the item, set 0 for items to remove
            NewQuantity = itemIdsToRemove.Contains(item.Id) ? 0 : int.TryParse(form[$"itemquantity{item.Id}"], out var quantity) ? quantity : item.Quantity,
            Item = item,
            Product = products.TryGetValue(item.ProductId, out var value) ? value : null
        }).Where(item => item.NewQuantity != item.Item.Quantity);

        //order cart items
        //first should be items with a reduced quantity and that require other products; or items with an increased quantity and are required for other products
        var orderedCart = await itemsWithNewQuantity
            .OrderByDescendingAwait(async cartItem =>
                (cartItem.NewQuantity < cartItem.Item.Quantity &&
                 (cartItem.Product?.RequireOtherProducts ?? false)) ||
                (cartItem.NewQuantity > cartItem.Item.Quantity && cartItem.Product != null && (await _shoppingCartService
                    .GetProductsRequiringProductAsync(cart, cartItem.Product)).Any()))
            .ToListAsync();

        //try to update cart items with new quantities and get warnings
        var warnings = await orderedCart.SelectAwait(async cartItem => new
        {
            ItemId = cartItem.Item.Id,
            Warnings = await _shoppingCartService.UpdateShoppingCartItemAsync(customer,
                cartItem.Item.Id, cartItem.Item.AttributesXml,
                cartItem.NewQuantity, true)
        }).ToListAsync();

        //updated cart
        cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        //prepare model
        var model = new ShoppingCartDto();
        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);

        //update current warnings
        foreach (var warningItem in warnings.Where(warningItem => warningItem.Warnings.Any()))
        {
            //find shopping cart item model to display appropriate warnings
            var itemModel = model.Items.FirstOrDefault(item => item.Id == warningItem.ItemId);
            if (itemModel != null)
                itemModel.Warnings = warningItem.Warnings.Concat(itemModel.Warnings).Distinct().ToList();
        }

        return Ok(model);
    }

    //[HttpPost, ActionName("Cart")]
    //[Route("ContinueShopping")]
    [FormValueRequired("continueshopping")]

    public virtual async Task<IActionResult> ContinueShopping()
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var returnUrl = await _genericAttributeService.GetAttributeAsync<string>(await _workContext.GetCurrentCustomerAsync(), NopCustomerDefaults.LastContinueShoppingPageAttribute, store.Id);

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToRoute("Homepage");
    }

    //[HttpPost, ActionName("Cart")]
    //[Route("StartCheckout")]
    [FormValueRequired("checkout")]

    public virtual async Task<IActionResult> StartCheckout(IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        //validate attributes
        var checkoutAttributes = await _genericAttributeService.GetAttributeAsync<string>(customer,
            NopCustomerDefaults.CheckoutAttributes, store.Id);
        var checkoutAttributeWarnings = await _shoppingCartService.GetShoppingCartWarningsAsync(cart, checkoutAttributes, true);
        if (checkoutAttributeWarnings.Any())
        {
            //something wrong, redisplay the page with warnings
            var model = new ShoppingCartDto();
            model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart, validateCheckoutAttributes: true);
            return Ok(model);
        }

        var anonymousPermissed = _orderSettings.AnonymousCheckoutAllowed
                                 && _customerSettings.UserRegistrationType == UserRegistrationType.Disabled;

        if (anonymousPermissed || !await _customerService.IsGuestAsync(customer))
            return Error(); // RedirectToRoute("Checkout");

        var cartProductIds = cart.Select(ci => ci.ProductId).ToArray();

        if (!_orderSettings.AnonymousCheckoutAllowed)
        {
            //verify user identity (it may be facebook login page, or google, or local)
            return Challenge();
        }

        return Ok(); // RedirectToRoute("LoginCheckoutAsGuest", new { returnUrl = Url.RouteUrl("ShoppingCart") });
    }

    [FormValueRequired("applydiscountcouponcode")]
    [HttpPost]
    [Route("ApplyDiscountCoupon", Name = "ApplyDiscountCoupon")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ApplyDiscountCoupon([FromQuery] string discountcouponcode, IFormCollection form)
    {
        //trim
        if (discountcouponcode != null)
            discountcouponcode = discountcouponcode.Trim();

        //cart
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        var model = new ShoppingCartDto();
        if (!string.IsNullOrWhiteSpace(discountcouponcode))
        {
            //we find even hidden records here. this way we can display a user-friendly message if it's expired
            var discounts = (await _discountService.GetAllDiscountsAsync(couponCode: discountcouponcode, showHidden: true))
                .Where(d => d.RequiresCouponCode)
                .ToList();
            if (discounts.Any())
            {
                var userErrors = new List<string>();
                var anyValidDiscount = await discounts.AnyAwaitAsync(async discount =>
                {
                    var validationResult = await _discountService.ValidateDiscountAsync(discount, customer, [discountcouponcode]);
                    userErrors.AddRange(validationResult.Errors);

                    return validationResult.IsValid;
                });

                if (anyValidDiscount)
                {
                    //valid
                    await _customerService.ApplyDiscountCouponCodeAsync(customer, discountcouponcode);
                    model.DiscountBox.Messages.Add(await _localizationService.GetResourceAsync("ShoppingCart.DiscountCouponCode.Applied"));
                    model.DiscountBox.IsApplied = true;
                }
                else
                {
                    if (userErrors.Any())
                        //some user errors
                        model.DiscountBox.Messages = userErrors;
                    else
                        //general error text
                        model.DiscountBox.Messages.Add(await _localizationService.GetResourceAsync("ShoppingCart.DiscountCouponCode.WrongDiscount"));
                }
            }
            else
                //discount cannot be found
                model.DiscountBox.Messages.Add(await _localizationService.GetResourceAsync("ShoppingCart.DiscountCouponCode.CannotBeFound"));
        }
        else
            //empty coupon code
            model.DiscountBox.Messages.Add(await _localizationService.GetResourceAsync("ShoppingCart.DiscountCouponCode.Empty"));

        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);

        return Ok(model);
    }


    [FormValueRequired("applygiftcardcouponcode")]
    [HttpPost]
    [Route("ApplyGiftCard", Name = "ApplyGiftCard")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ApplyGiftCard([FromQuery] string giftcardcouponcode, IFormCollection form)
    {
        //trim
        if (giftcardcouponcode != null)
            giftcardcouponcode = giftcardcouponcode.Trim();

        //cart
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        var model = new ShoppingCartDto();

        if (!string.IsNullOrWhiteSpace(giftcardcouponcode))
        {
            var giftCard = (await _giftCardService.GetAllGiftCardsAsync(giftCardCouponCode: giftcardcouponcode)).FirstOrDefault();
            var isGiftCardValid = giftCard != null && await _giftCardService.IsGiftCardValidAsync(giftCard);
            if (isGiftCardValid)
            {
                await _customerService.ApplyGiftCardCouponCodeAsync(customer, giftcardcouponcode);
                model.GiftCardBox.Message = await _localizationService.GetResourceAsync("ShoppingCart.GiftCardCouponCode.Applied");
                model.GiftCardBox.IsApplied = true;
            }
            else
            {
                model.GiftCardBox.Message = await _localizationService.GetResourceAsync("ShoppingCart.GiftCardCouponCode.WrongGiftCard");
                model.GiftCardBox.IsApplied = false;
            }
        }
        else
        {
            model.GiftCardBox.Message = await _localizationService.GetResourceAsync("ShoppingCart.GiftCardCouponCode.WrongGiftCard");
            model.GiftCardBox.IsApplied = false;
        }


        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);
        return Ok(model);
    }


    [HttpPost]
    [Route("GetEstimateShipping", Name = "GetEstimateShipping")]
    [ProducesResponseType(typeof(EstimateShippingResultDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> GetEstimateShipping(EstimateShippingDto model, IFormCollection form)
    {
        if (model == null)
            model = new EstimateShippingDto();

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

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
        //parse and save checkout attributes
        await ParseAndSaveCheckoutAttributesAsync(cart, form);

        var result = await _shoppingCartModelFactory.PrepareEstimateShippingResultModelAsync(cart, model, true);

        return Ok(result);
    }


    [FormValueRequired(FormValueRequirement.StartsWith, "removediscount-")]
    [HttpPost]
    [Route("RemoveDiscountCoupon", Name = "RemoveDiscountCoupon")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> RemoveDiscountCoupon(IFormCollection form)
    {
        var model = new ShoppingCartDto();

        //get discount identifier
        var discountId = 0;
        foreach (var formValue in form.Keys)
            if (formValue.StartsWith("removediscount-", StringComparison.InvariantCultureIgnoreCase))
                discountId = Convert.ToInt32(formValue["removediscount-".Length..]);
        var discount = await _discountService.GetDiscountByIdAsync(discountId);
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (discount != null)
            await _customerService.RemoveDiscountCouponCodeAsync(customer, discount.CouponCode);

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);
        return Ok(model);
    }


    [FormValueRequired(FormValueRequirement.StartsWith, "removegiftcard-")]
    [HttpPost]
    [Route("RemoveGiftCardCode", Name = "RemoveGiftCardCode")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> RemoveGiftCardCode(IFormCollection form)
    {
        var model = new ShoppingCartDto();

        //get gift card identifier
        var giftCardId = 0;
        foreach (var formValue in form.Keys)
            if (formValue.StartsWith("removegiftcard-", StringComparison.InvariantCultureIgnoreCase))
                giftCardId = Convert.ToInt32(formValue["removegiftcard-".Length..]);
        var gc = await _giftCardService.GetGiftCardByIdAsync(giftCardId);
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (gc != null)
            await _customerService.RemoveGiftCardCouponCodeAsync(customer, gc.GiftCardCouponCode);

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart);
        return Ok(model);
    }

    #endregion

    #region Wishlist
    [HttpGet]
    [Route("api-frontend/Wishlist/Wishlist", Name = "Wishlist")]
    [ProducesResponseType(typeof(WishlistDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Wishlist(Guid? customerGuid)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST))
            return Unauthorized();

        var customer = customerGuid.HasValue ?
            await _customerService.GetCustomerByGuidAsync(customerGuid.Value)
            : await _workContext.GetCurrentCustomerAsync();
        if (customer == null)
            return NotFound();

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);

        var model = new WishlistDto();
        model = await _shoppingCartModelFactory.PrepareWishlistDtoAsync(model, cart, !customerGuid.HasValue);
        return Ok(model);
    }


    [FormValueRequired("updatecart")]
    [HttpPost]
    [Route("api-frontend/Wishlist/UpdateWishlist", Name = "UpdateWishlist")]
    [ProducesResponseType(typeof(WishlistDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> UpdateWishlist(IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST))
            return Unauthorized();

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);

        var allIdsToRemove = form.ContainsKey("removefromcart")
            ? form["removefromcart"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList()
            : new List<int>();

        //current warnings <cart item identifier, warnings>
        var innerWarnings = new Dictionary<int, IList<string>>();
        foreach (var sci in cart)
        {
            var remove = allIdsToRemove.Contains(sci.Id);
            if (remove)
                await _shoppingCartService.DeleteShoppingCartItemAsync(sci);
            else
            {
                foreach (var formKey in form.Keys)
                    if (formKey.Equals($"itemquantity{sci.Id}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (int.TryParse(form[formKey], out var newQuantity))
                        {
                            var currSciWarnings = await _shoppingCartService.UpdateShoppingCartItemAsync(customer,
                                sci.Id, sci.AttributesXml, 
                                newQuantity, true);
                            innerWarnings.Add(sci.Id, currSciWarnings);
                        }

                        break;
                    }
            }
        }

        //updated wishlist
        cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);
        var model = new WishlistDto();
        model = await _shoppingCartModelFactory.PrepareWishlistDtoAsync(model, cart);
        //update current warnings
        foreach (var kvp in innerWarnings)
        {
            //kvp = <cart item identifier, warnings>
            var sciId = kvp.Key;
            var warnings = kvp.Value;
            //find model
            var sciModel = model.Items.FirstOrDefault(x => x.Id == sciId);
            if (sciModel != null)
                foreach (var w in warnings)
                    if (!sciModel.Warnings.Contains(w))
                        sciModel.Warnings.Add(w);
        }

        return Ok(model);
    }


    [FormValueRequired("addtocartbutton")]
    [HttpPut]
    [Route("api-frontend/Wishlist/AddItemsToCartFromWishlist", Name = "AddItemsToCartFromWishlist")]
    [ProducesResponseType(typeof(WishlistDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddItemsToCartFromWishlist(Guid? customerGuid, IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART))
            return Unauthorized();

        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST))
            return Unauthorized();

        var customer = await _workContext.GetCurrentCustomerAsync();
        var pageCustomer = customerGuid.HasValue
            ? await _customerService.GetCustomerByGuidAsync(customerGuid.Value)
            : customer;
        if (pageCustomer == null)
            return NotFound();

        var store = await _storeContext.GetCurrentStoreAsync();
        var pageCart = await _shoppingCartService.GetShoppingCartAsync(pageCustomer, ShoppingCartType.Wishlist, store.Id);

        var allWarnings = new List<string>();
        var countOfAddedItems = 0;
        var allIdsToAdd = form.ContainsKey("addtocart")
            ? form["addtocart"].ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
            : [];
        foreach (var sci in pageCart)
        {
            if (allIdsToAdd.Contains(sci.Id))
            {
                var product = await _productService.GetProductByIdAsync(sci.ProductId);

                var warnings = await _shoppingCartService.AddToCartAsync(customer,
                    product, ShoppingCartType.ShoppingCart,
                    store.Id,
                    sci.AttributesXml,sci.Quantity, true);
                if (!warnings.Any())
                    countOfAddedItems++;
                if (_shoppingCartSettings.MoveItemsFromWishlistToCart && //settings enabled
                    !customerGuid.HasValue && //own wishlist
                    !warnings.Any()) //no warnings ( already in the cart)
                {
                    //let's remove the item from wishlist
                    await _shoppingCartService.DeleteShoppingCartItemAsync(sci);
                }

                allWarnings.AddRange(warnings);
            }
        }

        if (countOfAddedItems > 0)
        {
            //redirect to the shopping cart page

            if (allWarnings.Any())
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Wishlist.AddToCart.Error"));
            }

            return RedirectToRoute("ShoppingCart");
        }
        else
        {
            _notificationService.WarningNotification(await _localizationService.GetResourceAsync("Wishlist.AddToCart.NoAddedItems"));
        }
        //no items added. redisplay the wishlist page

        if (allWarnings.Any())
        {
            _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Wishlist.AddToCart.Error"));
        }

        var cart = await _shoppingCartService.GetShoppingCartAsync(pageCustomer, ShoppingCartType.Wishlist, store.Id);

        var model = new WishlistDto();
        model = await _shoppingCartModelFactory.PrepareWishlistDtoAsync(model, cart, !customerGuid.HasValue);
        return Ok(model);
    }

    [HttpGet]
    [Route("api-frontend/Wishlist/EmailWishlist", Name = "EmailWishlist")]
    [ProducesResponseType(typeof(WishlistEmailAFriendDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> EmailWishlist()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST) || !_shoppingCartSettings.EmailWishlistEnabled)
            return Unauthorized();

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.Wishlist, store.Id);

        if (!cart.Any())
            return NotFound();

        var model = new WishlistEmailAFriendDto();
        model = await _shoppingCartModelFactory.PrepareWishlistEmailAFriendDtoAsync(model, false);
        return Ok(model);
    }


    [FormValueRequired("send-email")]
    //[ValidateCaptcha]

    [HttpPost]
    [Route("api-frontend/Wishlist/EmailWishlistSend", Name = "EmailWishlistSend")]
    [ProducesResponseType(typeof(WishlistEmailAFriendDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> EmailWishlistSend(WishlistEmailAFriendDto model, bool captchaValid)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_WISHLIST) || !_shoppingCartSettings.EmailWishlistEnabled)
            return Unauthorized();

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.Wishlist, store.Id);

        if (!cart.Any())
            return NotFound();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnEmailWishlistToFriendPage && !captchaValid)
        {
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        //check whether the current customer is guest and ia allowed to email wishlist
        if (await _customerService.IsGuestAsync(customer) && !_shoppingCartSettings.AllowAnonymousUsersToEmailWishlist)
        {
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Wishlist.EmailAFriend.OnlyRegisteredUsers"));
        }

        if (ModelState.IsValid)
        {
            //email
            await _workflowMessageService.SendWishlistEmailAFriendMessageAsync(customer,
                (await _workContext.GetWorkingLanguageAsync()).Id, model.YourEmailAddress,
                model.FriendEmail, _htmlFormatter.FormatText(model.PersonalMessage, false, true, false, false, false, false),string.Empty);

            model.SuccessfullySent = true;
            model.Result = await _localizationService.GetResourceAsync("Wishlist.EmailAFriend.SuccessfullySent");

            return Ok(model);
        }

        //If we got this far, something failed, redisplay form
        model = await _shoppingCartModelFactory.PrepareWishlistEmailAFriendDtoAsync(model, true);

        return Ok(model);
    }

    #endregion

    #region Components

    [HttpGet]
    [Route("GetMiniShoppingCart", Name = "GetMiniShoppingCart")]
    [ProducesResponseType(typeof(MiniShoppingCartDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMiniShoppingCart()
    {
        if (!_shoppingCartSettings.MiniShoppingCartEnabled)
            return Error(errorMessage:"Disabled form settings");

        if (!await _permissionService.AuthorizeAsync(StandardPermission.PublicStore.ENABLE_SHOPPING_CART))
            return Unauthorized();

        var model = await _shoppingCartModelFactory.PrepareMiniShoppingCartDtoAsync();
        return Ok(model);
    }

    [HttpGet]
    [Route("GetOrderSummary", Name = "GetOrderSummary")]
    [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrderSummary(bool? prepareAndDisplayOrderReviewData, ShoppingCartDto overriddenModel)
    {
        //use already prepared (shared) model
        if (overriddenModel != null)
            return Ok(overriddenModel);

        //if not passed, then create a new model
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

        var model = new ShoppingCartDto();
        model = await _shoppingCartModelFactory.PrepareShoppingCartDtoAsync(model, cart,
            isEditable: false,
            prepareAndDisplayOrderReviewData: prepareAndDisplayOrderReviewData.GetValueOrDefault());
        return Ok(model);
    }




    [HttpGet]
    [Route("GetOrderTotals", Name = "GetOrderTotals")]
    [ProducesResponseType(typeof(OrderTotalsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrderTotals(bool isEditable)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

        var model = await _shoppingCartModelFactory.PrepareOrderTotalsDtoAsync(cart, isEditable);
        return Ok(model);
    }



    [HttpGet]
    [Route("GetShoppingCartEstimateShipping", Name = "GetShoppingCartEstimateShipping")]
    [ProducesResponseType(typeof(EstimateShippingDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetShoppingCartEstimateShipping(bool? prepareAndDisplayOrderReviewData)
    {
        if (!_shippingSettings.EstimateShippingCartPageEnabled)
            return Content(string.Empty);

        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

        var model = await _shoppingCartModelFactory.PrepareEstimateShippingDtoAsync(cart);
        if (!model.Enabled)
            return Content(string.Empty);

        return Ok(model);
    }

    [HttpGet]
    [Route("GetSelectedCheckoutAttributes", Name = "GetSelectedCheckoutAttributes")]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSelectedCheckoutAttributes()
    {
        var attributes = await _shoppingCartModelFactory.FormatSelectedCheckoutAttributesAsync();
        return Ok(attributes);
    }
    #endregion
}
