using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Vendors;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.DTOs.Common;
using Nop.Api.DTOs.Sitemap;
using System.Net;
using Nop.Api.DTOs.Responses;
using Nop.Api.DTOs.Blogs;

namespace Nop.Api.Controllers;

public partial class CommonController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CommonSettings _commonSettings;
    protected readonly ICommonDtoFactory _commonModelFactory;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ISitemapDtoFactory _sitemapModelFactory;
    protected readonly IStoreContext _storeContext;
    protected readonly IVendorService _vendorService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly SitemapSettings _sitemapSettings;
    protected readonly SitemapXmlSettings _sitemapXmlSettings;
    protected readonly StoreInformationSettings _storeInformationSettings;
    protected readonly VendorSettings _vendorSettings;
    private readonly TaxSettings _taxSettings;

    #endregion

    #region Ctor

    public CommonController(CaptchaSettings captchaSettings,
        CommonSettings commonSettings,
        ICommonDtoFactory commonModelFactory,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        IGenericAttributeService genericAttributeService,
        IHtmlFormatter htmlFormatter,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ISitemapDtoFactory sitemapModelFactory,
        IStoreContext storeContext,
        IVendorService vendorService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        SitemapSettings sitemapSettings,
        SitemapXmlSettings sitemapXmlSettings,
        StoreInformationSettings storeInformationSettings,
        VendorSettings vendorSettings,
        TaxSettings taxSettings)
    {
        _captchaSettings = captchaSettings;
        _commonSettings = commonSettings;
        _commonModelFactory = commonModelFactory;
        _currencyService = currencyService;
        _customerActivityService = customerActivityService;
        _genericAttributeService = genericAttributeService;
        _htmlFormatter = htmlFormatter;
        _languageService = languageService;
        _localizationService = localizationService;
        _sitemapModelFactory = sitemapModelFactory;
        _storeContext = storeContext;
        _vendorService = vendorService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _sitemapSettings = sitemapSettings;
        _sitemapXmlSettings = sitemapXmlSettings;
        _storeInformationSettings = storeInformationSettings;
        _vendorSettings = vendorSettings;
        _taxSettings = taxSettings;
    }

    #endregion

    #region Methods

    //page not found
    [HttpGet]
    [Route("PageNotFound", Name = "PageNotFound")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual IActionResult PageNotFound()
    {
        Response.StatusCode = 404;
        Response.ContentType = "text/html";

        return Ok();
    }

    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("SetLanguage/{langid}", Name = "SetLanguage")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SetLanguage([FromRoute] int langid, [FromQuery] string returnUrl = "")
    {
        var language = await _languageService.GetLanguageByIdAsync(langid);
        if (!language?.Published ?? false)
            language = await _workContext.GetWorkingLanguageAsync();

        //home page
        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        //language part in URL
        if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
        {
            //remove current language code if it's already localized URL
            if ((await returnUrl.IsLocalizedUrlAsync(Request.PathBase, true)).IsLocalized)
                returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(Request.PathBase, true);

            //and add code of passed language
            returnUrl = returnUrl.AddLanguageSeoCodeToUrl(Request.PathBase, true, language);
        }

        await _workContext.SetWorkingLanguageAsync(language);

        //prevent open redirection attack
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        return Redirect(returnUrl);
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("SetCurrency/{customerCurrencyId}", Name = "SetCurrency")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SetCurrency([FromRoute] int customerCurrency, [FromQuery] string returnUrl = "")
    {
        var currency = await _currencyService.GetCurrencyByIdAsync(customerCurrency);
        if (currency != null)
            await _workContext.SetWorkingCurrencyAsync(currency);

        //home page
        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        //prevent open redirection attack
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        return Redirect(returnUrl);
    }

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpPost]
    [Route("SetTaxType", Name = "SetTaxType")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SetTaxType([FromQuery] int customerTaxType, [FromQuery] string returnUrl = "")
    {
        var taxDisplayType = (TaxDisplayType)Enum.ToObject(typeof(TaxDisplayType), customerTaxType);
        await _workContext.SetTaxDisplayTypeAsync(taxDisplayType);

        //home page
        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        //prevent open redirection attack
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = Url.RouteUrl("Homepage");

        return Redirect(returnUrl);
    }

    //contact us page
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpGet]
    [Route("ContactUs", Name = "ContactUs")]
    [ProducesResponseType(typeof(ContactUsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ContactUs()
    {
        var model = new ContactUsDto();
        model = await _commonModelFactory.PrepareContactUsDtoAsync(model, false);

        return Ok(model);
    }


    //[ValidateCaptcha]
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    [HttpPost]
    [Route("ContactUsSend", Name = "ContactUsSend")]
    [ProducesResponseType(typeof(ContactUsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ContactUsSend(ContactUsDto model, bool captchaValid)
    {
        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        model = await _commonModelFactory.PrepareContactUsDtoAsync(model, true);

        if (ModelState.IsValid)
        {
            var subject = _commonSettings.SubjectFieldOnContactUsForm ? model.Subject : null;
            var body = _htmlFormatter.FormatText(model.Enquiry, false, true, false, false, false, false);

            await _workflowMessageService.SendContactUsMessageAsync((await _workContext.GetWorkingLanguageAsync()).Id,
                model.Email, model.FullName, subject, body);

            model.SuccessfullySent = true;
            model.Result = await _localizationService.GetResourceAsync("ContactUs.YourEnquiryHasBeenSent");

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.ContactUs",
                await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ContactUs"));

            return Ok(model);
        }

        return Ok(model);
    }

    //contact vendor page
    [HttpGet]
    [Route("ContactVendor/{vendorId}", Name = "ContactVendor")]
    [ProducesResponseType(typeof(ContactVendorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ContactVendor([FromRoute] int vendorId)
    {
        if (!_vendorSettings.AllowCustomersToContactVendors)
            return Error(errorMessage: "Disabled from settings");

        var vendor = await _vendorService.GetVendorByIdAsync(vendorId);
        if (vendor == null || !vendor.Active || vendor.Deleted)
            return Error(errorMessage: "vendor is null , not active , or deleted");

        var model = new ContactVendorDto();
        model = await _commonModelFactory.PrepareContactVendorDtoAsync(model, vendor, false);

        return Ok(model);
    }


    //[ValidateCaptcha]
    [HttpPost]
    [Route("ContactVendorSend", Name = "ContactVendorSend")]
    [ProducesResponseType(typeof(ContactVendorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ContactVendorSend(ContactVendorDto model, bool captchaValid)
    {
        if (!_vendorSettings.AllowCustomersToContactVendors)
            return Error(errorMessage: "Disabled from settings");

        var vendor = await _vendorService.GetVendorByIdAsync(model.VendorId);
        if (vendor == null || !vendor.Active || vendor.Deleted)
            return Error(errorMessage: "vendor is null , not active , or deleted");

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        model = await _commonModelFactory.PrepareContactVendorDtoAsync(model, vendor, true);

        if (ModelState.IsValid)
        {
            var subject = _commonSettings.SubjectFieldOnContactUsForm ? model.Subject : null;
            var body = _htmlFormatter.FormatText(model.Enquiry, false, true, false, false, false, false);

            await _workflowMessageService.SendContactVendorMessageAsync(vendor, (await _workContext.GetWorkingLanguageAsync()).Id,
                model.Email, model.FullName, subject, body);

            model.SuccessfullySent = true;
            model.Result = await _localizationService.GetResourceAsync("ContactVendor.YourEnquiryHasBeenSent");

            return Ok(model);
        }

        return Ok(model);
    }

    //sitemap page
    [HttpPost]
    [Route("Sitemap", Name = "Sitemap")]
    [ProducesResponseType(typeof(SitemapDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Sitemap(SitemapPageDto pageModel)
    {
        if (!_sitemapSettings.SitemapEnabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _sitemapModelFactory.PrepareSitemapDtoAsync(pageModel);

        return Ok(model);
    }

    //SEO sitemap page
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("SitemapXml/{id}", Name = "SitemapXml")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SitemapXml([FromRoute] int? id)
    {
        if (!_sitemapXmlSettings.SitemapXmlEnabled)
            return StatusCode(StatusCodes.Status403Forbidden);

        try
        {
            var sitemapXmlModel = await _sitemapModelFactory.PrepareSitemapXmlDtoAsync(id ?? 0);
            return PhysicalFile(sitemapXmlModel.SitemapXmlPath, MimeTypes.ApplicationXml);
        }
        catch
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    //[HttpGet]
    //[Route("SetStoreTheme", Name = "SetStoreTheme")]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    //public virtual async Task<IActionResult> SetStoreTheme([FromQuery] string themeName, string returnUrl = "")
    //{
    //    //await _themeContext.SetWorkingThemeNameAsync(themeName);

    //    //home page
    //    if (string.IsNullOrEmpty(returnUrl))
    //        returnUrl = Url.RouteUrl("Homepage");

    //    //prevent open redirection attack
    //    if (!Url.IsLocalUrl(returnUrl))
    //        returnUrl = Url.RouteUrl("Homepage");

    //    return Redirect(returnUrl);
    //}


    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpGet]
    [Route("EuCookieLawAccept", Name = "EuCookieLawAccept")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> EuCookieLawAccept()
    {
        if (!_storeInformationSettings.DisplayEuCookieLawWarning)
            //disabled
            return Ok(new { stored = false });

        //save setting
        var store = await _storeContext.GetCurrentStoreAsync();
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(), NopCustomerDefaults.EuCookieLawAcceptedAttribute, true, store.Id);
        return Ok(new { stored = true });
    }

    //robots.txt file
    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("RobotsTextFile", Name = "RobotsTextFile")]
    [ProducesResponseType(typeof(RobotsTextFileResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> RobotsTextFile()
    {
        var robotsFileContent = await _commonModelFactory.PrepareRobotsTextFileAsync();

        return Content(robotsFileContent, MimeTypes.TextPlain);
    }

    //TODO: Check this back

    ////available even when a store is closed
    //[CheckAccessClosedStore(ignore: true)]
    ////available even when navigation is not allowed
    //[CheckAccessPublicStore(ignore: true)]
    //public virtual IActionResult GenericUrl()
    //{
    //    //seems that no entity was found
    //    return InvokeHttp404();
    //}

    ////store is closed
    ////available even when a store is closed
    //[CheckAccessClosedStore(ignore: true)]
    //public virtual IActionResult StoreClosed()
    //{
    //    return Ok();
    //}

    ////helper method to redirect users. Workaround for GenericPathRoute class where we're not allowed to do it
    //public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
    //{
    //    //ensure it's invoked from our GenericPathRoute class
    //    if (!HttpContext.Items.TryGetValue(NopHttpDefaults.GenericRouteInternalRedirect, out var value) || value is not bool redirect || !redirect)
    //    {
    //        url = Url.RouteUrl("Homepage");
    //        permanentRedirect = false;
    //    }

    //    //home page
    //    if (string.IsNullOrEmpty(url))
    //    {
    //        url = Url.RouteUrl("Homepage");
    //        permanentRedirect = false;
    //    }

    //    //prevent open redirection attack
    //    if (!Url.IsLocalUrl(url))
    //    {
    //        url = Url.RouteUrl("Homepage");
    //        permanentRedirect = false;
    //    }

    //    if (permanentRedirect)
    //        return RedirectPermanent(url);

    //    return Redirect(url);
    //}

    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    [HttpGet]
    [Route("FallbackRedirect", Name = "FallbackRedirect")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual IActionResult FallbackRedirect()
    {
        //nothing was found
        return InvokeHttp404();
    }

    #endregion

    #region Components
    [HttpGet]
    [Route("GetCurrencySelector", Name = "GetCurrencySelector")]
    [ProducesResponseType(typeof(CurrencySelectorDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCurrencySelector()
    {
        var model = await _commonModelFactory.PrepareCurrencySelectorDtoAsync();
        if (model.AvailableCurrencies.Count == 1)
            return Content("");

        return Ok(model);
    }

    [HttpGet]
    [Route("GetHeaderLinks", Name = "GetHeaderLinks")]
    [ProducesResponseType(typeof(HeaderLinksDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetHeaderLinks()
    {
        var model = await _commonModelFactory.PrepareHeaderLinksDtoAsync();
        return Ok(model);
    }


    [HttpGet]
    [Route("GetLanguageSelector", Name = "GetLanguageSelector")]
    [ProducesResponseType(typeof(LanguageSelectorDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLanguageSelector()
    {
        var model = await _commonModelFactory.PrepareLanguageSelectorDtoAsync();

        if (model.AvailableLanguages.Count == 1)
            return Content("");

        return Ok(model);
    }



    [HttpGet]
    [Route("GetSocialButtons", Name = "GetSocialButtons")]
    [ProducesResponseType(typeof(SocialDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSocialButtons()
    {
        var model = await _commonModelFactory.PrepareSocialDtoAsync();
        return Ok(model);
    }

    [HttpGet]
    [Route("GetTaxTypeSelector", Name = "GetTaxTypeSelector")]
    [ProducesResponseType(typeof(TaxTypeSelectorDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTaxTypeSelector()
    {
        if (!_taxSettings.AllowCustomersToSelectTaxDisplayType)
            return Content("");

        var model = await _commonModelFactory.PrepareTaxTypeSelectorDtoAsync();
        return Ok(model);
    }



    [HttpGet]
    [Route("GetFavicon", Name = "GetFavicon")]
    [ProducesResponseType(typeof(FaviconAndAppIconsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFavicon()
    {
        var model = await _commonModelFactory.PrepareFaviconAndAppIconsDtoAsync();
        if (string.IsNullOrEmpty(model.HeadCode))
            return Content("");
        return Ok(model);
    }



    //[HttpGet]
    //[Route("GetFooter", Name = "GetFooter")]
    //[ProducesResponseType(typeof(FooterDto), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetFooter()
    //{
    //    var model = await _commonModelFactory.PrepareFooterDtoAsync();
    //    return Ok(model);
    //}






    #endregion
}
