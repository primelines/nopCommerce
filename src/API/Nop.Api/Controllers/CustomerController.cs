using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Events;
using Nop.Core.Http;
using Nop.Core.Http.Extensions;
using Nop.Services.Attributes;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Authentication.MultiFactor;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Gdpr;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Tax;
using Nop.Api.Factories;
using Nop.Api.Framework;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.Framework.Validators;
using Nop.Api.DTOs.Customer;
using ILogger = Nop.Services.Logging.ILogger;
using System.Net;
using Nop.Api.DTOs.Responses;
using Nop.Api.DTOs.Polls;
using Nop.Api.DTOs.Profile;

namespace Nop.Api.Controllers;

public partial class CustomerController : BasePublicController
{
    #region Fields

    protected readonly AddressSettings _addressSettings;
    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CustomerSettings _customerSettings;
    protected readonly DateTimeSettings _dateTimeSettings;
    protected readonly ForumSettings _forumSettings;
    protected readonly GdprSettings _gdprSettings;
    protected readonly HtmlEncoder _htmlEncoder;
    protected readonly IAddressDtoFactory _addressModelFactory;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    protected readonly IAttributeParser<CustomerAttribute, CustomerAttributeValue> _customerAttributeParser;
    protected readonly IAttributeService<CustomerAttribute, CustomerAttributeValue> _customerAttributeService;
    protected readonly IAuthenticationService _authenticationService;
    protected readonly ICountryService _countryService;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerDtoFactory _customerModelFactory;
    protected readonly ICustomerRegistrationService _customerRegistrationService;
    protected readonly ICustomerService _customerService;
    protected readonly IDownloadService _downloadService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IExportManager _exportManager;
    protected readonly IExternalAuthenticationService _externalAuthenticationService;
    protected readonly IGdprService _gdprService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IGiftCardService _giftCardService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;
    protected readonly IMultiFactorAuthenticationPluginManager _multiFactorAuthenticationPluginManager;
    protected readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    protected readonly INotificationService _notificationService;
    protected readonly IOrderService _orderService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IProductService _productService;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly IStoreContext _storeContext;
    protected readonly ITaxService _taxService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly MediaSettings _mediaSettings;
    protected readonly MultiFactorAuthenticationSettings _multiFactorAuthenticationSettings;
    protected readonly StoreInformationSettings _storeInformationSettings;
    protected readonly TaxSettings _taxSettings;
    private readonly IProfileDtoFactory _profileDtoFactory;
    private readonly IExternalAuthenticationDtoFactory _externalAuthenticationDtoFactory;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public CustomerController(AddressSettings addressSettings,
        CaptchaSettings captchaSettings,
        CustomerSettings customerSettings,
        DateTimeSettings dateTimeSettings,
        ForumSettings forumSettings,
        GdprSettings gdprSettings,
        HtmlEncoder htmlEncoder,
        IAddressDtoFactory addressModelFactory,
        IAddressService addressService,
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
        IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
        IAuthenticationService authenticationService,
        ICountryService countryService,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        ICustomerDtoFactory customerModelFactory,
        ICustomerRegistrationService customerRegistrationService,
        ICustomerService customerService,
        IDownloadService downloadService,
        IEventPublisher eventPublisher,
        IExportManager exportManager,
        IExternalAuthenticationService externalAuthenticationService,
        IGdprService gdprService,
        IGenericAttributeService genericAttributeService,
        IGiftCardService giftCardService,
        ILocalizationService localizationService,
        ILogger logger,
        IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
        INewsLetterSubscriptionService newsLetterSubscriptionService,
        INotificationService notificationService,
        IOrderService orderService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceFormatter priceFormatter,
        IProductService productService,
        IStateProvinceService stateProvinceService,
        IStoreContext storeContext,
        ITaxService taxService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        MediaSettings mediaSettings,
        MultiFactorAuthenticationSettings multiFactorAuthenticationSettings,
        StoreInformationSettings storeInformationSettings,
        TaxSettings taxSettings,
        IProfileDtoFactory profileDtoFactory,
        IExternalAuthenticationDtoFactory externalAuthenticationDtoFactory)
    {
        _addressSettings = addressSettings;
        _captchaSettings = captchaSettings;
        _customerSettings = customerSettings;
        _dateTimeSettings = dateTimeSettings;
        _forumSettings = forumSettings;
        _gdprSettings = gdprSettings;
        _htmlEncoder = htmlEncoder;
        _addressModelFactory = addressModelFactory;
        _addressService = addressService;
        _addressAttributeParser = addressAttributeParser;
        _customerAttributeParser = customerAttributeParser;
        _customerAttributeService = customerAttributeService;
        _authenticationService = authenticationService;
        _countryService = countryService;
        _currencyService = currencyService;
        _customerActivityService = customerActivityService;
        _customerModelFactory = customerModelFactory;
        _customerRegistrationService = customerRegistrationService;
        _customerService = customerService;
        _downloadService = downloadService;
        _eventPublisher = eventPublisher;
        _exportManager = exportManager;
        _externalAuthenticationService = externalAuthenticationService;
        _gdprService = gdprService;
        _genericAttributeService = genericAttributeService;
        _giftCardService = giftCardService;
        _localizationService = localizationService;
        _logger = logger;
        _multiFactorAuthenticationPluginManager = multiFactorAuthenticationPluginManager;
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _notificationService = notificationService;
        _orderService = orderService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _priceFormatter = priceFormatter;
        _productService = productService;
        _stateProvinceService = stateProvinceService;
        _storeContext = storeContext;
        _taxService = taxService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _mediaSettings = mediaSettings;
        _multiFactorAuthenticationSettings = multiFactorAuthenticationSettings;
        _storeInformationSettings = storeInformationSettings;
        _taxSettings = taxSettings;
        _profileDtoFactory = profileDtoFactory;
        _externalAuthenticationDtoFactory = externalAuthenticationDtoFactory;
    }

    #endregion

    #region Utilities

    protected virtual void ValidateRequiredConsents(List<GdprConsent> consents, IFormCollection form)
    {
        foreach (var consent in consents)
        {
            var controlId = $"consent{consent.Id}";
            var cbConsent = form[controlId];
            if (StringValues.IsNullOrEmpty(cbConsent) || !cbConsent.ToString().Equals("on"))
            {
                ModelState.AddModelError("", consent.RequiredMessage);
            }
        }
    }

    protected virtual async Task<string> ParseSelectedProviderAsync(IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var store = await _storeContext.GetCurrentStoreAsync();

        var multiFactorAuthenticationProviders = await _multiFactorAuthenticationPluginManager.LoadActivePluginsAsync(await _workContext.GetCurrentCustomerAsync(), store.Id);
        foreach (var provider in multiFactorAuthenticationProviders)
        {
            var controlId = $"provider_{provider.PluginDescriptor.SystemName}";

            var curProvider = form[controlId];
            if (!StringValues.IsNullOrEmpty(curProvider))
            {
                var selectedProvider = curProvider.ToString();
                if (!string.IsNullOrEmpty(selectedProvider))
                {
                    return selectedProvider;
                }
            }
        }
        return string.Empty;
    }

    protected virtual async Task<string> ParseCustomCustomerAttributesAsync(IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var attributesXml = "";
        var attributes = await _customerAttributeService.GetAllAttributesAsync();
        foreach (var attribute in attributes)
        {
            var controlId = $"{NopCustomerServicesDefaults.CustomerAttributePrefix}{attribute.Id}";
            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                {
                    var ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var selectedAttributeId = int.Parse(ctrlAttributes);
                        if (selectedAttributeId > 0)
                            attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
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
                                attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }
                }
                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                {
                    //load read-only (already server-side selected) values
                    var attributeValues = await _customerAttributeService.GetAttributeValuesAsync(attribute.Id);
                    foreach (var selectedAttributeId in attributeValues
                                 .Where(v => v.IsPreSelected)
                                 .Select(v => v.Id)
                                 .ToList())
                    {
                        attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
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
                        attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                            attribute, enteredText);
                    }
                }
                    break;
                case AttributeControlType.Datepicker:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                case AttributeControlType.FileUpload:
                //not supported customer attributes
                default:
                    break;
            }
        }

        return attributesXml;
    }

    protected virtual async Task LogGdprAsync(Customer customer, CustomerInfoDto oldCustomerInfoDto,
        CustomerInfoDto newCustomerInfoDto, IFormCollection form)
    {
        try
        {
            //consents
            var consents = (await _gdprService.GetAllConsentsAsync()).Where(consent => consent.DisplayOnCustomerInfoPage).ToList();
            foreach (var consent in consents)
            {
                var previousConsentValue = await _gdprService.IsConsentAcceptedAsync(consent.Id, customer.Id);
                var controlId = $"consent{consent.Id}";
                var cbConsent = form[controlId];
                if (!StringValues.IsNullOrEmpty(cbConsent) && cbConsent.ToString().Equals("on"))
                {
                    //agree
                    if (!previousConsentValue.HasValue || !previousConsentValue.Value)
                    {
                        await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentAgree, consent.Message);
                    }
                }
                else
                {
                    //disagree
                    if (!previousConsentValue.HasValue || previousConsentValue.Value)
                    {
                        await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentDisagree, consent.Message);
                    }
                }
            }

            //newsletter subscriptions
            if (_gdprSettings.LogNewsletterConsent)
            {
                if (oldCustomerInfoDto.Newsletter && !newCustomerInfoDto.Newsletter)
                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentDisagree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
                if (!oldCustomerInfoDto.Newsletter && newCustomerInfoDto.Newsletter)
                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentAgree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
            }

            //user profile changes
            if (!_gdprSettings.LogUserProfileChanges)
                return;

            if (oldCustomerInfoDto.Gender != newCustomerInfoDto.Gender)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Gender")} = {newCustomerInfoDto.Gender}");

            if (oldCustomerInfoDto.FirstName != newCustomerInfoDto.FirstName)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.FirstName")} = {newCustomerInfoDto.FirstName}");

            if (oldCustomerInfoDto.LastName != newCustomerInfoDto.LastName)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.LastName")} = {newCustomerInfoDto.LastName}");

            if (oldCustomerInfoDto.ParseDateOfBirth() != newCustomerInfoDto.ParseDateOfBirth())
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.DateOfBirth")} = {newCustomerInfoDto.ParseDateOfBirth()}");

            if (oldCustomerInfoDto.Email != newCustomerInfoDto.Email)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Email")} = {newCustomerInfoDto.Email}");

            if (oldCustomerInfoDto.Company != newCustomerInfoDto.Company)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Company")} = {newCustomerInfoDto.Company}");

            if (oldCustomerInfoDto.StreetAddress != newCustomerInfoDto.StreetAddress)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StreetAddress")} = {newCustomerInfoDto.StreetAddress}");

            if (oldCustomerInfoDto.StreetAddress2 != newCustomerInfoDto.StreetAddress2)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StreetAddress2")} = {newCustomerInfoDto.StreetAddress2}");

            if (oldCustomerInfoDto.ZipPostalCode != newCustomerInfoDto.ZipPostalCode)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.ZipPostalCode")} = {newCustomerInfoDto.ZipPostalCode}");

            if (oldCustomerInfoDto.City != newCustomerInfoDto.City)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.City")} = {newCustomerInfoDto.City}");

            if (oldCustomerInfoDto.County != newCustomerInfoDto.County)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.County")} = {newCustomerInfoDto.County}");

            if (oldCustomerInfoDto.CountryId != newCustomerInfoDto.CountryId)
            {
                var countryName = (await _countryService.GetCountryByIdAsync(newCustomerInfoDto.CountryId))?.Name;
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Country")} = {countryName}");
            }

            if (oldCustomerInfoDto.StateProvinceId != newCustomerInfoDto.StateProvinceId)
            {
                var stateProvinceName = (await _stateProvinceService.GetStateProvinceByIdAsync(newCustomerInfoDto.StateProvinceId))?.Name;
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StateProvince")} = {stateProvinceName}");
            }
        }
        catch (Exception exception)
        {
            await _logger.ErrorAsync(exception.Message, exception, customer);
        }
    }

    #endregion

    #region Methods

    #region Login / logout

    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpGet]
    [Route("Logout", Name = "Logout")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Login(bool? checkoutAsGuest)
    {
        var model = await _customerModelFactory.PrepareLoginDtoAsync(checkoutAsGuest);
        var customer = await _workContext.GetCurrentCustomerAsync();

        if (await _customerService.IsRegisteredAsync(customer))
        {
            var fullName = await _customerService.GetCustomerFullNameAsync(customer);
            var message = await _localizationService.GetResourceAsync("Account.Login.AlreadyLogin");
            _notificationService.SuccessNotification(string.Format(message, _htmlEncoder.Encode(fullName)));
        }

        return Ok(model);
    }

    [HttpPost]
    [Route("Login")]
    //[ValidateCaptcha]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]

    public virtual async Task<IActionResult> Login(LoginDto model, string returnUrl, bool captchaValid)
    {
        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            var customerUserName = model.Username;
            var customerEmail = model.Email;
            var userNameOrEmail = _customerSettings.UsernamesEnabled ? customerUserName : customerEmail;

            var loginResult = await _customerRegistrationService.ValidateCustomerAsync(userNameOrEmail, model.Password);
            switch (loginResult)
            {
                case CustomerLoginResults.Successful:
                {
                    var customer = _customerSettings.UsernamesEnabled
                        ? await _customerService.GetCustomerByUsernameAsync(customerUserName)
                        : await _customerService.GetCustomerByEmailAsync(customerEmail);

                    return await _customerRegistrationService.SignInCustomerAsync(customer, returnUrl, model.RememberMe);
                }
                case CustomerLoginResults.MultiFactorAuthenticationRequired:
                {
                    var customerMultiFactorAuthenticationInfo = new CustomerMultiFactorAuthenticationInfo
                    {
                        UserName = userNameOrEmail,
                        RememberMe = model.RememberMe,
                        ReturnUrl = returnUrl
                    };
                    await HttpContext.Session.SetAsync(
                        NopCustomerDefaults.CustomerMultiFactorAuthenticationInfo,
                        customerMultiFactorAuthenticationInfo);
                    return RedirectToRoute("MultiFactorVerification");
                }
                case CustomerLoginResults.CustomerNotExist:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.CustomerNotExist"));
                    break;
                case CustomerLoginResults.Deleted:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.Deleted"));
                    break;
                case CustomerLoginResults.NotActive:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotActive"));
                    break;
                case CustomerLoginResults.NotRegistered:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotRegistered"));
                    break;
                case CustomerLoginResults.LockedOut:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.LockedOut"));
                    break;
                case CustomerLoginResults.WrongPassword:
                default:
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials"));
                    break;
            }
        }

        //If we got this far, something failed, redisplay form
        model = await _customerModelFactory.PrepareLoginDtoAsync(model.CheckoutAsGuest);
        return Ok(model);
    }

    //TODO: Check back

    ///// <summary>
    ///// The entry point for injecting a plugin component of type "MultiFactorAuth"
    ///// </summary>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the user verification page for Multi-factor authentication. Served by an authentication provider.
    ///// </returns>
    //public virtual async Task<IActionResult> MultiFactorVerification()
    //{
    //    if (!await _multiFactorAuthenticationPluginManager.HasActivePluginsAsync())
    //        return RedirectToRoute("Login");

    //    var customerMultiFactorAuthenticationInfo = await HttpContext.Session.GetAsync<CustomerMultiFactorAuthenticationInfo>(
    //        NopCustomerDefaults.CustomerMultiFactorAuthenticationInfo);
    //    var userName = customerMultiFactorAuthenticationInfo?.UserName;
    //    if (string.IsNullOrEmpty(userName))
    //        return RedirectToRoute("Homepage");

    //    var customer = _customerSettings.UsernamesEnabled ? await _customerService.GetCustomerByUsernameAsync(userName) : await _customerService.GetCustomerByEmailAsync(userName);
    //    if (customer == null)
    //        return RedirectToRoute("Homepage");

    //    if (!await _permissionService.AuthorizeAsync(StandardPermission.EnableMultiFactorAuthentication, customer))
    //        return RedirectToRoute("Homepage");

    //    var selectedProvider = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute);
    //    if (string.IsNullOrEmpty(selectedProvider))
    //        return RedirectToRoute("Homepage");

    //    var model = new MultiFactorAuthenticationProviderDto();
    //    model = await _customerModelFactory.PrepareMultiFactorAuthenticationProviderDtoAsync(model, selectedProvider, true);

    //    return Ok(model);
    //}

    ////available even when a store is closed
    //[CheckAccessClosedStore(ignore: true)]
    ////available even when navigation is not allowed
    //[CheckAccessPublicStore(ignore: true)]
    //public virtual async Task<IActionResult> Logout()
    //{
    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    if (_workContext.OriginalCustomerIfImpersonated != null)
    //    {
    //        //activity log
    //        await _customerActivityService.InsertActivityAsync(_workContext.OriginalCustomerIfImpersonated, "Impersonation.Finished",
    //            string.Format(await _localizationService.GetResourceAsync("ActivityLog.Impersonation.Finished.StoreOwner"),
    //                customer.Email, customer.Id),
    //            customer);

    //        await _customerActivityService.InsertActivityAsync("Impersonation.Finished",
    //            string.Format(await _localizationService.GetResourceAsync("ActivityLog.Impersonation.Finished.Customer"),
    //                _workContext.OriginalCustomerIfImpersonated.Email, _workContext.OriginalCustomerIfImpersonated.Id),
    //            _workContext.OriginalCustomerIfImpersonated);

    //        //logout impersonated customer
    //        await _genericAttributeService
    //            .SaveAttributeAsync<int?>(_workContext.OriginalCustomerIfImpersonated, NopCustomerDefaults.ImpersonatedCustomerIdAttribute, null);

    //        //redirect back to customer details page (admin area)
    //        return RedirectToAction("Edit", "Customer", new { id = customer.Id, area = AreaNames.ADMIN });
    //    }

    //    //activity log
    //    await _customerActivityService.InsertActivityAsync(customer, "PublicStore.Logout",
    //        await _localizationService.GetResourceAsync("ActivityLog.PublicStore.Logout"), customer);

    //    //standard logout 
    //    await _authenticationService.SignOutAsync();

    //    //raise logged out event       
    //    await _eventPublisher.PublishAsync(new CustomerLoggedOutEvent(customer));

    //    //EU Cookie
    //    if (_storeInformationSettings.DisplayEuCookieLawWarning)
    //    {
    //        //the cookie law message should not pop up immediately after logout.
    //        //otherwise, the user will have to click it again...
    //        //and thus next visitor will not click it... so violation for that cookie law..
    //        //the only good solution in this case is to store a temporary variable
    //        //indicating that the EU cookie popup window should not be displayed on the next page open (after logout redirection to homepage)
    //        //but it'll be displayed for further page loads
    //        //TempData[$"{NopCookieDefaults.Prefix}{NopCookieDefaults.IgnoreEuCookieLawWarning}"] = true;
    //    }

    //    return RedirectToRoute("Homepage");
    //}

    #endregion

    #region Password recovery

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpGet]
    [Route("PasswordRecovery", Name = "PasswordRecovery")]
    [ProducesResponseType(typeof(PasswordRecoveryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> PasswordRecovery()
    {
        var model = new PasswordRecoveryDto();
        model = await _customerModelFactory.PreparePasswordRecoveryModelAsync(model);

        return Ok(model);
    }

    //[ValidateCaptcha]
    [FormValueRequired("send-email")]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpPost]
    [Route("PasswordRecoverySend", Name = "PasswordRecoverySend")]
    [ProducesResponseType(typeof(PasswordRecoveryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> PasswordRecoverySend(PasswordRecoveryDto model, bool captchaValid)
    {
        // validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnForgotPasswordPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            var customer = await _customerService.GetCustomerByEmailAsync(model.Email);
            if (customer != null && customer.Active && !customer.Deleted)
            {
                //save token and current date
                var passwordRecoveryToken = Guid.NewGuid();
                await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.PasswordRecoveryTokenAttribute,
                    passwordRecoveryToken.ToString());
                DateTime? generatedDateTime = DateTime.UtcNow;
                await _genericAttributeService.SaveAttributeAsync(customer,
                    NopCustomerDefaults.PasswordRecoveryTokenDateGeneratedAttribute, generatedDateTime);

                //send email
                await _workflowMessageService.SendCustomerPasswordRecoveryMessageAsync(customer,
                    (await _workContext.GetWorkingLanguageAsync()).Id);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.PasswordRecovery.EmailHasBeenSent"));
            }
            else
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Account.PasswordRecovery.EmailNotFound"));
            }
        }

        model = await _customerModelFactory.PreparePasswordRecoveryModelAsync(model);

        return Ok(model);
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpPost]
    [Route("PasswordRecoveryConfirm", Name = "PasswordRecoveryConfirm")]
    [ProducesResponseType(typeof(PasswordRecoveryConfirmDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PasswordRecoveryConfirm([FromQuery] string token, [FromQuery] string email, [FromQuery] Guid guid)
    {
        //For backward compatibility with previous versions where email was used as a parameter in the URL
        var customer = await _customerService.GetCustomerByEmailAsync(email)
                       ?? await _customerService.GetCustomerByGuidAsync(guid);

        if (customer == null)
            return NotFound();

        var model = new PasswordRecoveryConfirmDto { ReturnUrl = Url.RouteUrl("Homepage") };
        if (string.IsNullOrEmpty(await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.PasswordRecoveryTokenAttribute)))
        {
            model.DisablePasswordChanging = true;
            model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.PasswordAlreadyHasBeenChanged");
            return Ok(model);
        }

        //validate token
        if (!await _customerService.IsPasswordRecoveryTokenValidAsync(customer, token))
        {
            model.DisablePasswordChanging = true;
            model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.WrongToken");
            return Ok(model);
        }

        //validate token expiration date
        if (await _customerService.IsPasswordRecoveryLinkExpiredAsync(customer))
        {
            model.DisablePasswordChanging = true;
            model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.LinkExpired");
            return Ok(model);
        }

        return Ok(model);
    }


    [FormValueRequired("set-password")]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpPost]
    [Route("PasswordRecoveryConfirmPOST", Name = "PasswordRecoveryConfirmPOST")]
    [ProducesResponseType(typeof(PasswordRecoveryConfirmDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PasswordRecoveryConfirmPOST([FromQuery] string token, [FromQuery] string email, [FromQuery] Guid guid, PasswordRecoveryConfirmDto model)
    {
        //For backward compatibility with previous versions where email was used as a parameter in the URL
        var customer = await _customerService.GetCustomerByEmailAsync(email)
                       ?? await _customerService.GetCustomerByGuidAsync(guid);

        if (customer == null)
            return NotFound();

        model.ReturnUrl = Url.RouteUrl("Homepage");

        //validate token
        if (!await _customerService.IsPasswordRecoveryTokenValidAsync(customer, token))
        {
            model.DisablePasswordChanging = true;
            model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.WrongToken");
            return Ok(model);
        }

        //validate token expiration date
        if (await _customerService.IsPasswordRecoveryLinkExpiredAsync(customer))
        {
            model.DisablePasswordChanging = true;
            model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.LinkExpired");
            return Ok(model);
        }

        if (!ModelState.IsValid)
            return Ok(model);

        var response = await _customerRegistrationService
            .ChangePasswordAsync(new ChangePasswordRequest(customer.Email, false, _customerSettings.DefaultPasswordFormat, model.NewPassword));
        if (!response.Success)
        {
            model.Result = string.Join(';', response.Errors);
            return Ok(model);
        }

        await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.PasswordRecoveryTokenAttribute, "");

        //authenticate customer after changing password
        await _customerRegistrationService.SignInCustomerAsync(customer, null, true);

        model.DisablePasswordChanging = true;
        model.Result = await _localizationService.GetResourceAsync("Account.PasswordRecovery.PasswordHasBeenChanged");
        return Ok(model);
    }

    #endregion     

    #region Register

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpGet]
    [Route("Register", Name = "Register")]
    [ProducesResponseType(typeof(RegisterDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Register(string returnUrl)
    {
        //check whether registration is allowed
        if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled, returnUrl });

        var model = new RegisterDto();
        model = await _customerModelFactory.PrepareRegisterDtoAsync(model, false, setDefaultValues: true);

        return Ok(model);
    }

    [HttpPost]
    [Route("Register")]
    //[ValidateCaptcha]
    //[ValidateHoneypot]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]

    public virtual async Task<IActionResult> Register(RegisterDto model, string returnUrl, bool captchaValid, IFormCollection form)
    {
        //check whether registration is allowed
        if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled, returnUrl });

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsRegisteredAsync(customer))
        {
            //Already registered customer. 
            await _authenticationService.SignOutAsync();

            //raise logged out event       
            await _eventPublisher.PublishAsync(new CustomerLoggedOutEvent(customer));

            customer = await _customerService.InsertGuestCustomerAsync();

            //Save a new record
            await _workContext.SetCurrentCustomerAsync(customer);
        }

        var store = await _storeContext.GetCurrentStoreAsync();
        customer.RegisteredInStoreId = store.Id;

        //custom customer attributes
        var customerAttributesXml = await ParseCustomCustomerAttributesAsync(form);
        var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
        foreach (var error in customerAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        //GDPR
        if (_gdprSettings.GdprEnabled)
        {
            var consents = (await _gdprService
                .GetAllConsentsAsync()).Where(consent => consent.DisplayDuringRegistration && consent.IsRequired).ToList();

            ValidateRequiredConsents(consents, form);
        }

        if (ModelState.IsValid)
        {
            var customerUserName = model.Username;
            var customerEmail = model.Email;

            var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
            var registrationRequest = new CustomerRegistrationRequest(customer,
                customerEmail,
                _customerSettings.UsernamesEnabled ? customerUserName : customerEmail,
                model.Password,
                _customerSettings.DefaultPasswordFormat,
                store.Id,
                isApproved);
            var registrationResult = await _customerRegistrationService.RegisterCustomerAsync(registrationRequest);
            if (registrationResult.Success)
            {
                //properties
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    customer.TimeZoneId = model.TimeZoneId;

                //VAT number
                if (_taxSettings.EuVatEnabled)
                {
                    customer.VatNumber = model.VatNumber;

                    var (vatNumberStatus, _, vatAddress) = await _taxService.GetVatNumberStatusAsync(model.VatNumber);
                    customer.VatNumberStatusId = (int)vatNumberStatus;
                    //send VAT number admin notification
                    if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                        await _workflowMessageService.SendNewVatSubmittedStoreOwnerNotificationAsync(customer, model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                }

                //form fields
                if (_customerSettings.GenderEnabled)
                    customer.Gender = model.Gender;
                if (_customerSettings.FirstNameEnabled)
                    customer.FirstName = model.FirstName;
                if (_customerSettings.LastNameEnabled)
                    customer.LastName = model.LastName;
                if (_customerSettings.DateOfBirthEnabled)
                    customer.DateOfBirth = model.ParseDateOfBirth();
                if (_customerSettings.CompanyEnabled)
                    customer.Company = model.Company;
                if (_customerSettings.StreetAddressEnabled)
                    customer.StreetAddress = model.StreetAddress;
                if (_customerSettings.StreetAddress2Enabled)
                    customer.StreetAddress2 = model.StreetAddress2;
                if (_customerSettings.ZipPostalCodeEnabled)
                    customer.ZipPostalCode = model.ZipPostalCode;
                if (_customerSettings.CityEnabled)
                    customer.City = model.City;
                if (_customerSettings.CountyEnabled)
                    customer.County = model.County;
                if (_customerSettings.CountryEnabled)
                    customer.CountryId = model.CountryId;
                if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                    customer.StateProvinceId = model.StateProvinceId;
                if (_customerSettings.PhoneEnabled)
                    customer.Phone = model.Phone;
                if (_customerSettings.FaxEnabled)
                    customer.Fax = model.Fax;

                //save customer attributes
                customer.CustomCustomerAttributesXML = customerAttributesXml;
                await _customerService.UpdateCustomerAsync(customer);

                //newsletter
                if (_customerSettings.NewsletterEnabled)
                {
                    var isNewsletterActive = _customerSettings.UserRegistrationType != UserRegistrationType.EmailValidation;

                    //save newsletter value
                    var newsletters = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionsByEmailAsync(customerEmail, store.Id);
                   foreach(var newsletter in newsletters)
                    {
                        if (newsletter != null)
                        {
                            if (model.Newsletter)
                            {
                                newsletter.Active = isNewsletterActive;
                                await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(newsletter);

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentAgree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
                                }
                            }
                            //else
                            //{
                            //When registering, not checking the newsletter check box should not take an existing email address off of the subscription list.
                            //_newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);
                            //}
                        }
                        else
                        {
                            if (model.Newsletter)
                            {
                                await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = customerEmail,
                                    Active = isNewsletterActive,
                                    StoreId = store.Id,
                                    LanguageId = customer.LanguageId ?? store.DefaultLanguageId,
                                    CreatedOnUtc = DateTime.UtcNow
                                });

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentAgree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
                                }
                            }
                        }
                    }


                }

                if (_customerSettings.AcceptPrivacyPolicyEnabled)
                {
                    //privacy policy is required
                    //GDPR
                    if (_gdprSettings.GdprEnabled && _gdprSettings.LogPrivacyPolicyConsent)
                    {
                        await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentAgree, await _localizationService.GetResourceAsync("Gdpr.Consent.PrivacyPolicy"));
                    }
                }

                //GDPR
                if (_gdprSettings.GdprEnabled)
                {
                    var consents = (await _gdprService.GetAllConsentsAsync()).Where(consent => consent.DisplayDuringRegistration).ToList();
                    foreach (var consent in consents)
                    {
                        var controlId = $"consent{consent.Id}";
                        var cbConsent = form[controlId];
                        if (!StringValues.IsNullOrEmpty(cbConsent) && cbConsent.ToString().Equals("on"))
                        {
                            //agree
                            await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentAgree, consent.Message);
                        }
                        else
                        {
                            //disagree
                            await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentDisagree, consent.Message);
                        }
                    }
                }

                //insert default address (if possible)
                var defaultAddress = new Address
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Company = customer.Company,
                    CountryId = customer.CountryId > 0
                        ? (int?)customer.CountryId
                        : null,
                    StateProvinceId = customer.StateProvinceId > 0
                        ? (int?)customer.StateProvinceId
                        : null,
                    County = customer.County,
                    City = customer.City,
                    Address1 = customer.StreetAddress,
                    Address2 = customer.StreetAddress2,
                    ZipPostalCode = customer.ZipPostalCode,
                    PhoneNumber = customer.Phone,
                    FaxNumber = customer.Fax,
                    CreatedOnUtc = customer.CreatedOnUtc
                };
                if (await _addressService.IsAddressValidAsync(defaultAddress))
                {
                    //some validation
                    if (defaultAddress.CountryId == 0)
                        defaultAddress.CountryId = null;
                    if (defaultAddress.StateProvinceId == 0)
                        defaultAddress.StateProvinceId = null;
                    //set default address
                    //customer.Addresses.Add(defaultAddress);

                    await _addressService.InsertAddressAsync(defaultAddress);

                    await _customerService.InsertCustomerAddressAsync(customer, defaultAddress);

                    customer.BillingAddressId = defaultAddress.Id;
                    customer.ShippingAddressId = defaultAddress.Id;

                    await _customerService.UpdateCustomerAsync(customer);
                }

                //notifications
                if (_customerSettings.NotifyNewCustomerRegistration)
                    await _workflowMessageService.SendCustomerRegisteredStoreOwnerNotificationMessageAsync(customer,
                        _localizationSettings.DefaultAdminLanguageId);

                //raise event       
                await _eventPublisher.PublishAsync(new CustomerRegisteredEvent(customer));
                var currentLanguage = await _workContext.GetWorkingLanguageAsync();

                switch (_customerSettings.UserRegistrationType)
                {
                    case UserRegistrationType.EmailValidation:
                        //email validation message
                        await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
                        await _workflowMessageService.SendCustomerEmailValidationMessageAsync(customer, currentLanguage.Id);

                        //result
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation, returnUrl });

                    case UserRegistrationType.AdminApproval:
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval, returnUrl });

                    case UserRegistrationType.Standard:
                        //send customer welcome message
                        await _workflowMessageService.SendCustomerWelcomeMessageAsync(customer, currentLanguage.Id);

                        //raise event       
                        await _eventPublisher.PublishAsync(new CustomerActivatedEvent(customer));

                        returnUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard, returnUrl });
                        return await _customerRegistrationService.SignInCustomerAsync(customer, returnUrl, true);

                    default:
                        return RedirectToRoute("Homepage");
                }
            }

            //errors
            foreach (var error in registrationResult.Errors)
                ModelState.AddModelError("", error);
        }

        //If we got this far, something failed, redisplay form
        model = await _customerModelFactory.PrepareRegisterDtoAsync(model, true, customerAttributesXml);

        return Ok(model);
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("RegisterResult/{resultId}", Name = "RegisterResult")]
    [ProducesResponseType(typeof(RegisterResultDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]

    public virtual async Task<IActionResult> RegisterResult([FromRoute] int resultId, [FromQuery] string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        var model = await _customerModelFactory.PrepareRegisterResultDtoAsync(resultId, returnUrl);
        return Ok(model);
    }


    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpGet]
    [Route("CheckUsernameAvailability", Name = "CheckUsernameAvailability")]
    [ProducesResponseType(typeof(CheckUsernameAvailabilityResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> CheckUsernameAvailability([FromQuery] string username)
    {
        var usernameAvailable = false;
        var statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.NotAvailable");

        if (!UsernamePropertyValidator<string, string>.IsValid(username, _customerSettings))
        {
            statusText = await _localizationService.GetResourceAsync("Account.Fields.Username.NotValid");
        }
        else if (_customerSettings.UsernamesEnabled && !string.IsNullOrWhiteSpace(username))
        {
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            if (currentCustomer != null &&
                currentCustomer.Username != null &&
                currentCustomer.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.CurrentUsername");
            }
            else
            {
                var customer = await _customerService.GetCustomerByUsernameAsync(username);
                if (customer == null)
                {
                    statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.Available");
                    usernameAvailable = true;
                }
            }
        }

        return Ok(new { Available = usernameAvailable, Text = statusText });
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("AccountActivation", Name = "AccountActivation")]
    [ProducesResponseType(typeof(AccountActivationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> AccountActivation([FromQuery] string token, [FromQuery] string email, [FromQuery] Guid guid)
    {
        //For backward compatibility with previous versions where email was used as a parameter in the URL
        var customer = await _customerService.GetCustomerByEmailAsync(email)
                       ?? await _customerService.GetCustomerByGuidAsync(guid);

        if (customer == null)
            return NotFound();

        var model = new AccountActivationDto { ReturnUrl = Url.RouteUrl("Homepage") };
        var cToken = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.AccountActivationTokenAttribute);
        if (string.IsNullOrEmpty(cToken))
        {
            model.Result = await _localizationService.GetResourceAsync("Account.AccountActivation.AlreadyActivated");
            return Ok(model);
        }

        if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
            return Error();

        //activate user account
        customer.Active = true;
        await _customerService.UpdateCustomerAsync(customer);
        await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.AccountActivationTokenAttribute, "");

        //send welcome message
        await _workflowMessageService.SendCustomerWelcomeMessageAsync(customer, (await _workContext.GetWorkingLanguageAsync()).Id);

        //raise event       
        await _eventPublisher.PublishAsync(new CustomerActivatedEvent(customer));

        //authenticate customer after activation
        await _customerRegistrationService.SignInCustomerAsync(customer, null, true);

        //activating newsletter if need
        var store = await _storeContext.GetCurrentStoreAsync();
        var newsletters = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionsByEmailAsync(customer.Email, store.Id);

        foreach (var newsletter in newsletters)
        {
            if (newsletter != null && !newsletter.Active)
            {
                newsletter.Active = true;
                await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(newsletter);
            }
        }


        model.Result = await _localizationService.GetResourceAsync("Account.AccountActivation.Activated");
        return Ok(model);
    }

    #endregion

    #region My account / Info


    [HttpGet]
    [Route("CustomerInfo", Name = "CustomerInfo")]
    [ProducesResponseType(typeof(CustomerInfoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]

    public virtual async Task<IActionResult> Info()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        var model = new CustomerInfoDto();
        model = await _customerModelFactory.PrepareCustomerInfoDtoAsync(model, customer, false);

        return Ok(model);
    }

    //[HttpPost]
    //[Route("Info", Name = "CustomerInfo")]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [HttpPost]
    [Route("CustomerInfo", Name = "CustomerInfo")]
    [ProducesResponseType(typeof(CustomerInfoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Info(CustomerInfoDto model, IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        var oldCustomerDto = new CustomerInfoDto();

        //get customer info model before changes for gdpr log
        if (_gdprSettings.GdprEnabled & _gdprSettings.LogUserProfileChanges)
            oldCustomerDto = await _customerModelFactory.PrepareCustomerInfoDtoAsync(oldCustomerDto, customer, false);

        //custom customer attributes
        var customerAttributesXml = await ParseCustomCustomerAttributesAsync(form);
        var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
        foreach (var error in customerAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        //GDPR
        if (_gdprSettings.GdprEnabled)
        {
            var consents = (await _gdprService
                .GetAllConsentsAsync()).Where(consent => consent.DisplayOnCustomerInfoPage && consent.IsRequired).ToList();

            ValidateRequiredConsents(consents, form);
        }

        try
        {
            if (ModelState.IsValid)
            {
                //username 
                if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
                {
                    var userName = model.Username;
                    if (!customer.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //change username
                        await _customerRegistrationService.SetUsernameAsync(customer, userName);

                        //re-authenticate
                        //do not authenticate users in impersonation mode
                        if (_workContext.OriginalCustomerIfImpersonated == null)
                            await _authenticationService.SignInAsync(customer, true);
                    }
                }
                //email
                var email = model.Email;
                if (!customer.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                {
                    //change email
                    var requireValidation = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                    await _customerRegistrationService.SetEmailAsync(customer, email, requireValidation);

                    //do not authenticate users in impersonation mode
                    if (_workContext.OriginalCustomerIfImpersonated == null)
                    {
                        //re-authenticate (if usernames are disabled)
                        if (!_customerSettings.UsernamesEnabled && !requireValidation)
                            await _authenticationService.SignInAsync(customer, true);
                    }
                }

                //properties
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    customer.TimeZoneId = model.TimeZoneId;
                //VAT number
                if (_taxSettings.EuVatEnabled)
                {
                    var prevVatNumber = customer.VatNumber;
                    customer.VatNumber = model.VatNumber;

                    if (prevVatNumber != model.VatNumber)
                    {
                        var (vatNumberStatus, _, vatAddress) = await _taxService.GetVatNumberStatusAsync(model.VatNumber);
                        customer.VatNumberStatusId = (int)vatNumberStatus;

                        //send VAT number admin notification
                        if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                            await _workflowMessageService.SendNewVatSubmittedStoreOwnerNotificationAsync(customer,
                                model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                    }
                }

                //form fields
                if (_customerSettings.GenderEnabled)
                    customer.Gender = model.Gender;
                if (_customerSettings.FirstNameEnabled)
                    customer.FirstName = model.FirstName;
                if (_customerSettings.LastNameEnabled)
                    customer.LastName = model.LastName;
                if (_customerSettings.DateOfBirthEnabled)
                    customer.DateOfBirth = model.ParseDateOfBirth();
                if (_customerSettings.CompanyEnabled)
                    customer.Company = model.Company;
                if (_customerSettings.StreetAddressEnabled)
                    customer.StreetAddress = model.StreetAddress;
                if (_customerSettings.StreetAddress2Enabled)
                    customer.StreetAddress2 = model.StreetAddress2;
                if (_customerSettings.ZipPostalCodeEnabled)
                    customer.ZipPostalCode = model.ZipPostalCode;
                if (_customerSettings.CityEnabled)
                    customer.City = model.City;
                if (_customerSettings.CountyEnabled)
                    customer.County = model.County;
                if (_customerSettings.CountryEnabled)
                    customer.CountryId = model.CountryId;
                if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                    customer.StateProvinceId = model.StateProvinceId;
                if (_customerSettings.PhoneEnabled)
                    customer.Phone = model.Phone;
                if (_customerSettings.FaxEnabled)
                    customer.Fax = model.Fax;

                customer.CustomCustomerAttributesXML = customerAttributesXml;
                await _customerService.UpdateCustomerAsync(customer);

                //newsletter
                if (_customerSettings.NewsletterEnabled)
                {
                    //save newsletter value
                    var store = await _storeContext.GetCurrentStoreAsync();
                    var newsletters = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionsByEmailAsync(customer.Email, store.Id);
                    foreach(var newsletter in newsletters)
                    {
                        if (newsletter != null)
                        {
                            if (model.Newsletter)
                            {
                                newsletter.Active = true;
                                await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(newsletter);
                            }
                            else
                            {
                                await _newsLetterSubscriptionService.DeleteNewsLetterSubscriptionAsync(newsletter);
                            }
                        }
                        else
                        {
                            if (model.Newsletter)
                            {
                                await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = customer.Email,
                                    Active = true,
                                    StoreId = store.Id,
                                    LanguageId = customer.LanguageId ?? store.DefaultLanguageId,
                                    CreatedOnUtc = DateTime.UtcNow
                                });
                            }
                        }
                    }

                }

                if (_forumSettings.ForumsEnabled && _forumSettings.SignaturesEnabled)
                    await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.SignatureAttribute, model.Signature);

                //GDPR
                if (_gdprSettings.GdprEnabled)
                    await LogGdprAsync(customer, oldCustomerDto, model, form);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.CustomerInfo.Updated"));

                return RedirectToRoute("CustomerInfo");
            }
        }
        catch (Exception exc)
        {
            ModelState.AddModelError("", exc.Message);
        }

        //If we got this far, something failed, redisplay form
        model = await _customerModelFactory.PrepareCustomerInfoDtoAsync(model, customer, true, customerAttributesXml);

        return Ok(model);
    }



    [HttpDelete]
    [Route("RemoveExternalAssociation/{id}", Name = "RemoveExternalAssociation")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]

    public virtual async Task<IActionResult> RemoveExternalAssociation([FromRoute] int id)
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        //ensure it's our record
        var ear = await _externalAuthenticationService.GetExternalAuthenticationRecordByIdAsync(id);

        if (ear == null)
        {
            return Ok(new
            {
                redirect = Url.RouteUrl("CustomerInfo"),
            });
        }

        await _externalAuthenticationService.DeleteExternalAuthenticationRecordAsync(ear);

        return Ok(new
        {
            redirect = Url.RouteUrl("CustomerInfo"),
        });
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("EmailRevalidation", Name = "EmailRevalidation")]
    [ProducesResponseType(typeof(EmailRevalidationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> EmailRevalidation([FromQuery] string token, [FromQuery] string email, [FromQuery] Guid guid)
    {
        //For backward compatibility with previous versions where email was used as a parameter in the URL
        var customer = await _customerService.GetCustomerByEmailAsync(email)
                       ?? await _customerService.GetCustomerByGuidAsync(guid);

        if (customer == null)
            return NotFound();

        var model = new EmailRevalidationDto { ReturnUrl = Url.RouteUrl("Homepage") };
        var cToken = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.EmailRevalidationTokenAttribute);
        if (string.IsNullOrEmpty(cToken))
        {
            model.Result = await _localizationService.GetResourceAsync("Account.EmailRevalidation.AlreadyChanged");
            return Ok(model);
        }

        if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
            return Error();

        if (string.IsNullOrEmpty(customer.EmailToRevalidate))
            return Error();

        if (_customerSettings.UserRegistrationType != UserRegistrationType.EmailValidation)
            return Error();

        //change email
        try
        {
            await _customerRegistrationService.SetEmailAsync(customer, customer.EmailToRevalidate, false);
        }
        catch (Exception exc)
        {
            model.Result = await _localizationService.GetResourceAsync(exc.Message);
            return Ok(model);
        }

        customer.EmailToRevalidate = null;
        await _customerService.UpdateCustomerAsync(customer);
        await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.EmailRevalidationTokenAttribute, "");

        //authenticate customer after changing email
        await _customerRegistrationService.SignInCustomerAsync(customer, null, true);

        model.Result = await _localizationService.GetResourceAsync("Account.EmailRevalidation.Changed");
        return Ok(model);
    }

    #endregion

    #region My account / Addresses

    [HttpGet]
    [Route("Addresses", Name = "Addresses")]
    [ProducesResponseType(typeof(CustomerAddressListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Addresses()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var model = await _customerModelFactory.PrepareCustomerAddressListDtoAsync();

        return Ok(model);
    }


    [HttpDelete]
    [Route("AddressDelete/{addressId}", Name = "AddressDelete")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddressDelete([FromRoute] int addressId)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        //find address (ensure that it belongs to the current customer)
        var address = await _customerService.GetCustomerAddressAsync(customer.Id, addressId);
        if (address != null)
        {
            await _customerService.RemoveCustomerAddressAsync(customer, address);
            await _customerService.UpdateCustomerAsync(customer);
            //now delete the address record
            await _addressService.DeleteAddressAsync(address);
        }

        //redirect to the address list page
        return Ok(new
        {
            redirect = Url.RouteUrl("CustomerAddresses"),
        });
    }

    [HttpGet]
    [Route("AddressAdd", Name = "AddressAdd")]
    [ProducesResponseType(typeof(CustomerAddressEditDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddressAdd()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var model = new CustomerAddressEditDto();
        await _addressModelFactory.PrepareAddressDtoAsync(model.Address,
            address: null,
            excludeProperties: false,
            addressSettings: _addressSettings,
            loadCountries: async () => await _countryService.GetAllCountriesAsync((await _workContext.GetWorkingLanguageAsync()).Id));

        return Ok(model);
    }

    [HttpPost]
    [Route("AddressAdd", Name = "AddressAdd")]
    [ProducesResponseType(typeof(CustomerAddressEditDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> AddressAdd(CustomerAddressEditDto model, IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        //custom address attributes
        var customAttributes = await _addressAttributeParser.ParseCustomAttributesAsync(form, NopCommonDefaults.AddressAttributeControlName);
        var customAttributeWarnings = await _addressAttributeParser.GetAttributeWarningsAsync(customAttributes);
        foreach (var error in customAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        if (ModelState.IsValid)
        {
            var address = model.Address.ToEntity();
            address.CustomAttributes = customAttributes;
            address.CreatedOnUtc = DateTime.UtcNow;
            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;


            await _addressService.InsertAddressAsync(address);

            await _customerService.InsertCustomerAddressAsync(customer, address);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.CustomerAddresses.Added"));

            return RedirectToRoute("CustomerAddresses");
        }

        //If we got this far, something failed, redisplay form
        await _addressModelFactory.PrepareAddressDtoAsync(model.Address,
            address: null,
            excludeProperties: true,
            addressSettings: _addressSettings,
            loadCountries: async () => await _countryService.GetAllCountriesAsync((await _workContext.GetWorkingLanguageAsync()).Id),
            overrideAttributesXml: customAttributes);

        return Ok(model);
    }

    [HttpGet]
    [Route("AddressEdit/{addressId}", Name = "AddressEdit")]
    [ProducesResponseType(typeof(CustomerAddressEditDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> AddressEdit([FromRoute] int addressId)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        //find address (ensure that it belongs to the current customer)
        var address = await _customerService.GetCustomerAddressAsync(customer.Id, addressId);
        if (address == null)
            //address is not found
            return RedirectToRoute("CustomerAddresses");

        var model = new CustomerAddressEditDto();
        await _addressModelFactory.PrepareAddressDtoAsync(model.Address,
            address: address,
            excludeProperties: false,
            addressSettings: _addressSettings,
            loadCountries: async () => await _countryService.GetAllCountriesAsync((await _workContext.GetWorkingLanguageAsync()).Id));

        return Ok(model);
    }

    [HttpPut]
    [Route("AddressEdit", Name = "AddressEditX")]
    [ProducesResponseType(typeof(AddressEditResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> AddressEdit(CustomerAddressEditDto model, IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        //find address (ensure that it belongs to the current customer)
        var address = await _customerService.GetCustomerAddressAsync(customer.Id, model.Address.Id);
        if (address == null)
            //address is not found
            return RedirectToRoute("CustomerAddresses");

        //custom address attributes
        var customAttributes = await _addressAttributeParser.ParseCustomAttributesAsync(form, NopCommonDefaults.AddressAttributeControlName);
        var customAttributeWarnings = await _addressAttributeParser.GetAttributeWarningsAsync(customAttributes);
        foreach (var error in customAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        if (ModelState.IsValid)
        {
            address = model.Address.ToEntity(address);
            address.CustomAttributes = customAttributes;
            await _addressService.UpdateAddressAsync(address);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.CustomerAddresses.Updated"));

            return RedirectToRoute("CustomerAddresses");
        }

        //If we got this far, something failed, redisplay form
        await _addressModelFactory.PrepareAddressDtoAsync(model.Address,
            address: address,
            excludeProperties: true,
            addressSettings: _addressSettings,
            loadCountries: async () => await _countryService.GetAllCountriesAsync((await _workContext.GetWorkingLanguageAsync()).Id),
            overrideAttributesXml: customAttributes);

        return Ok(model);
    }

    #endregion

    #region My account / Change password

    [HttpGet]
    [Route("ChangePassword", Name = "ChangePassword")]
    [ProducesResponseType(typeof(ChangePasswordDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ChangePassword()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        var model = await _customerModelFactory.PrepareChangePasswordDtoAsync();

        //display the cause of the change password 
        if (await _customerService.IsPasswordExpiredAsync(customer))
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Account.ChangePassword.PasswordIsExpired"));

        return Ok(model);
    }

    [HttpPost]
    [Route("ChangePassword", Name = "ChangePassword")]
    [ProducesResponseType(typeof(ChangePasswordDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ChangePassword(ChangePasswordDto model, string returnUrl)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (ModelState.IsValid)
        {
            var changePasswordRequest = new ChangePasswordRequest(customer.Email,
                true, _customerSettings.DefaultPasswordFormat, model.NewPassword, model.OldPassword);
            var changePasswordResult = await _customerRegistrationService.ChangePasswordAsync(changePasswordRequest);
            if (changePasswordResult.Success)
            {
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.ChangePassword.Success"));

                //authenticate customer after changing password
                await _customerRegistrationService.SignInCustomerAsync(customer, null, true);

                if (string.IsNullOrEmpty(returnUrl))
                    return Ok(model);

                //prevent open redirection attack
                if (!Url.IsLocalUrl(returnUrl))
                    returnUrl = Url.RouteUrl("Homepage");

                return new RedirectResult(returnUrl);
            }

            //errors
            foreach (var error in changePasswordResult.Errors)
                ModelState.AddModelError("", error);
        }

        //If we got this far, something failed, redisplay form
        return Ok(model);
    }

    #endregion

    #region My account / Avatar

    [HttpGet]
    [Route("Avatar", Name = "Avatar")]
    [ProducesResponseType(typeof(CustomerAvatarDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Avatar()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        if (!_customerSettings.AllowCustomersToUploadAvatars)
            return RedirectToRoute("CustomerInfo");

        var model = new CustomerAvatarDto();
        model = await _customerModelFactory.PrepareCustomerAvatarDtoAsync(model);

        return Ok(model);
    }

    //TODO: defrent parameters ([FromQuery]  fileName[FromQuery]  contentType)


    [FormValueRequired("upload-avatar")]

    [HttpPost]
    [Route("UploadAvatar", Name = "UploadAvatar")]
    [ProducesResponseType(typeof(CustomerAvatarDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]

    public virtual async Task<IActionResult> UploadAvatar(CustomerAvatarDto model, IFormFile uploadedFile)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (!_customerSettings.AllowCustomersToUploadAvatars)
            return RedirectToRoute("CustomerInfo");

        var contentType = uploadedFile?.ContentType.ToLowerInvariant();

        if (contentType != null && !contentType.Equals("image/jpeg") && !contentType.Equals("image/gif"))
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Avatar.UploadRules"));

        if (ModelState.IsValid)
        {
            try
            {
                var customerAvatar = await _pictureService.GetPictureByIdAsync(await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.AvatarPictureIdAttribute));
                if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
                {
                    var avatarMaxSize = _customerSettings.AvatarMaximumSizeBytes;
                    if (uploadedFile.Length > avatarMaxSize)
                        throw new NopException(string.Format(await _localizationService.GetResourceAsync("Account.Avatar.MaximumUploadedFileSize"), avatarMaxSize));

                    var customerPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);
                    if (customerAvatar != null)
                        customerAvatar = await _pictureService.UpdatePictureAsync(customerAvatar.Id, customerPictureBinary, contentType, null);
                    else
                        customerAvatar = await _pictureService.InsertPictureAsync(customerPictureBinary, contentType, null);
                }

                var customerAvatarId = 0;
                if (customerAvatar != null)
                    customerAvatarId = customerAvatar.Id;

                await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.AvatarPictureIdAttribute, customerAvatarId);

                model.AvatarUrl = await _pictureService.GetPictureUrlAsync(
                    await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.AvatarPictureIdAttribute),
                    _mediaSettings.AvatarPictureSize,
                    false);

                return Ok(model);
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", exc.Message);
            }
        }

        //If we got this far, something failed, redisplay form
        model = await _customerModelFactory.PrepareCustomerAvatarDtoAsync(model);
        return Ok(model);
    }


    [FormValueRequired("remove-avatar")]

    [HttpDelete]
    [Route("RemoveAvatar", Name = "RemoveAvatar")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> RemoveAvatar(CustomerAvatarDto model)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (!_customerSettings.AllowCustomersToUploadAvatars)
            return Error(); // RedirectToRoute("CustomerInfo");

        var customerAvatar = await _pictureService.GetPictureByIdAsync(await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.AvatarPictureIdAttribute));
        if (customerAvatar != null)
            await _pictureService.DeletePictureAsync(customerAvatar);
        await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.AvatarPictureIdAttribute, 0);

        return Ok(); // RedirectToRoute("CustomerAvatar");
    }

    #endregion

    #region GDPR tools

    [HttpGet]
    [Route("GdprTools", Name = "GdprTools")]
    [ProducesResponseType(typeof(GdprToolsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GdprTools()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        if (!_gdprSettings.GdprEnabled)
            return RedirectToRoute("CustomerInfo");

        var model = await _customerModelFactory.PrepareGdprToolsDtoAsync();

        return Ok(model);
    }


    [FormValueRequired("export-data")]

    [HttpGet]
    [Route("GdprToolsExport", Name = "GdprToolsExport")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GdprToolsExport()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (!_gdprSettings.GdprEnabled)
            return RedirectToRoute("CustomerInfo");

        //log
        await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ExportData, await _localizationService.GetResourceAsync("Gdpr.Exported"));

        var store = await _storeContext.GetCurrentStoreAsync();

        //export
        var bytes = await _exportManager.ExportCustomerGdprInfoToXlsxAsync(customer, store.Id);

        return File(bytes, MimeTypes.TextXlsx, "customerdata.xlsx");
    }


    [FormValueRequired("delete-account")]

    [HttpDelete]
    [Route("GdprToolsDelete", Name = "GdprToolsDelete")]
    [ProducesResponseType(typeof(GdprToolsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GdprToolsDelete()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (!_gdprSettings.GdprEnabled)
            return RedirectToRoute("CustomerInfo");

        //log
        await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.DeleteCustomer, await _localizationService.GetResourceAsync("Gdpr.DeleteRequested"));

        await _workflowMessageService.SendDeleteCustomerRequestStoreOwnerNotificationAsync(customer, _localizationSettings.DefaultAdminLanguageId);

        var model = await _customerModelFactory.PrepareGdprToolsDtoAsync();
        model.Result = await _localizationService.GetResourceAsync("Gdpr.DeleteRequested.Success");

        return Ok(model);
    }

    #endregion

    #region Check gift card balance

    //check gift card balance page
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpGet]
    [Route("CheckGiftCardBalance", Name = "CheckGiftCardBalance")]
    [ProducesResponseType(typeof(CheckGiftCardBalanceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> CheckGiftCardBalance()
    {
        if (!(_captchaSettings.Enabled && _customerSettings.AllowCustomersToCheckGiftCardBalance))
        {
            return RedirectToRoute("CustomerInfo");
        }

        var model = await _customerModelFactory.PrepareCheckGiftCardBalanceDtoAsync();

        return Ok(model);
    }


    [FormValueRequired("checkbalancegiftcard")]
    //[ValidateCaptcha]
    [HttpPost]
    [Route("CheckBalance", Name = "CheckBalance")]
    [ProducesResponseType(typeof(CheckGiftCardBalanceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> CheckBalance(CheckGiftCardBalanceDto model, bool captchaValid)
    {
        //validate CAPTCHA
        if (_captchaSettings.Enabled && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            var giftCard = (await _giftCardService.GetAllGiftCardsAsync(giftCardCouponCode: model.GiftCardCode)).FirstOrDefault();
            if (giftCard != null && await _giftCardService.IsGiftCardValidAsync(giftCard))
            {
                var remainingAmount = await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(await _giftCardService.GetGiftCardRemainingAmountAsync(giftCard), await _workContext.GetWorkingCurrencyAsync());
                model.Result = await _priceFormatter.FormatPriceAsync(remainingAmount, true, false);
            }
            else
            {
                model.Message = await _localizationService.GetResourceAsync("CheckGiftCardBalance.GiftCardCouponCode.Invalid");
            }
        }

        return Ok(model);
    }

    #endregion

    //TODO: No routs for this section

    //#region Multi-factor Authentication

    ////available even when a store is closed
    //[CheckAccessClosedStore(ignore: true)]
    //public virtual async Task<IActionResult> MultiFactorAuthentication()
    //{
    //    if (!await _multiFactorAuthenticationPluginManager.HasActivePluginsAsync())
    //    {
    //        return RedirectToRoute("CustomerInfo");
    //    }

    //    if (!await _permissionService.AuthorizeAsync(StandardPermission.EnableMultiFactorAuthentication))
    //        return RedirectToRoute("CustomerInfo");

    //    var model = new MultiFactorAuthenticationDto();
    //    model = await _customerModelFactory.PrepareMultiFactorAuthenticationDtoAsync(model);
    //    return Ok(model);
    //}

    //[HttpPost]
    //[Route("MultiFactorAuthentication")]

    //public virtual async Task<IActionResult> MultiFactorAuthentication(MultiFactorAuthenticationDto model, IFormCollection form)
    //{
    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    if (!await _customerService.IsRegisteredAsync(customer))
    //        return Challenge();

    //    if (!await _permissionService.AuthorizeAsync(StandardPermission.EnableMultiFactorAuthentication))
    //        return RedirectToRoute("CustomerInfo");

    //    try
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            //save MultiFactorIsEnabledAttribute
    //            if (!model.IsEnabled)
    //            {
    //                if (!_multiFactorAuthenticationSettings.ForceMultifactorAuthentication)
    //                {
    //                    await _genericAttributeService
    //                        .SaveAttributeAsync(customer, NopCustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute, string.Empty);

    //                    //raise change multi-factor authentication provider event       
    //                    await _eventPublisher.PublishAsync(new CustomerChangeMultiFactorAuthenticationProviderEvent(customer));
    //                }
    //                else
    //                {
    //                    model = await _customerModelFactory.PrepareMultiFactorAuthenticationDtoAsync(model);
    //                    model.Message = await _localizationService.GetResourceAsync("Account.MultiFactorAuthentication.Warning.ForceActivation");
    //                    return Ok(model);
    //                }
    //            }
    //            else
    //            {
    //                //save selected multi-factor authentication provider
    //                var selectedProvider = await ParseSelectedProviderAsync(form);
    //                var lastSavedProvider = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute);
    //                if (string.IsNullOrEmpty(selectedProvider) && !string.IsNullOrEmpty(lastSavedProvider))
    //                {
    //                    selectedProvider = lastSavedProvider;
    //                }

    //                if (selectedProvider != lastSavedProvider)
    //                {
    //                    await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute, selectedProvider);

    //                    //raise change multi-factor authentication provider event       
    //                    await _eventPublisher.PublishAsync(new CustomerChangeMultiFactorAuthenticationProviderEvent(customer));
    //                }
    //            }

    //            return RedirectToRoute("MultiFactorAuthenticationSettings");
    //        }
    //    }
    //    catch (Exception exc)
    //    {
    //        ModelState.AddModelError("", exc.Message);
    //    }

    //    //If we got this far, something failed, redisplay form
    //    model = await _customerModelFactory.PrepareMultiFactorAuthenticationDtoAsync(model);
    //    return Ok(model);
    //}

    //public virtual async Task<IActionResult> ConfigureMultiFactorAuthenticationProvider(string providerSysName)
    //{
    //    if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
    //        return Challenge();

    //    if (!await _permissionService.AuthorizeAsync(StandardPermission.EnableMultiFactorAuthentication))
    //        return RedirectToRoute("CustomerInfo");

    //    var model = new MultiFactorAuthenticationProviderDto();
    //    model = await _customerModelFactory.PrepareMultiFactorAuthenticationProviderDtoAsync(model, providerSysName);

    //    return Ok(model);
    //}

    //#endregion

    #endregion


    #region Components

    [HttpGet]
    [Route("GetCustomerNavigation", Name = "GetCustomerNavigation")]
    [ProducesResponseType(typeof(CustomerNavigationDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCustomerNavigation(int selectedTabId = 0)
    {
        var model = await _customerModelFactory.PrepareCustomerNavigationDtoAsync(selectedTabId);
        return Ok(model);
    }

    [HttpGet]
    [Route("GetProfileInfo", Name = "GetProfileInfo")]
    [ProducesResponseType(typeof(ProfileInfoDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProfileInfo(int customerProfileId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerProfileId);
        ArgumentNullException.ThrowIfNull(customer);

        var model = await _profileDtoFactory.PrepareProfileInfoDtoAsync(customer);
        return Ok(model);
    }



    [HttpGet]
    [Route("GetProfilePosts", Name = "GetProfilePosts")]
    [ProducesResponseType(typeof(ProfilePostsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProfilePosts(int customerProfileId, int pageNumber)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerProfileId);
        ArgumentNullException.ThrowIfNull(customer);

        var model = await _profileDtoFactory.PrepareProfilePostsDtoAsync(customer, pageNumber);
        return Ok(model);
    }




    [HttpGet]
    [Route("GetExternalMethods", Name = "GetExternalMethods")]
    [ProducesResponseType(typeof(List<ExternalAuthenticationMethodDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetExternalMethods()
    {
        var model = await _externalAuthenticationDtoFactory.PrepareExternalMethodsModelAsync();

        return Ok(model);
    }

    #endregion
}
