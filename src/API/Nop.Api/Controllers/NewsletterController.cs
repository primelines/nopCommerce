using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Security;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc.Filters;
using System.Net;
using Nop.Api.DTOs.Newsletter;
using Nop.Core.Domain.Customers;

namespace Nop.Api.Controllers;

public partial class NewsletterController : BasePublicController
{
    protected readonly CaptchaSettings _captchaSettings;
    protected readonly ICustomerService _customerService;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INewsletterDtoFactory _newsletterModelFactory;
    protected readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    private readonly CustomerSettings _customerSettings;

    public NewsletterController(CaptchaSettings captchaSettings,
        ICustomerService customerService,
        ILanguageService languageService,
        ILocalizationService localizationService,
        INewsletterDtoFactory newsletterModelFactory,
        INewsLetterSubscriptionService newsLetterSubscriptionService,
        IStoreContext storeContext,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        CustomerSettings customerSettings)
    {
        _captchaSettings = captchaSettings;
        _customerService = customerService;
        _languageService = languageService;
        _localizationService = localizationService;
        _newsletterModelFactory = newsletterModelFactory;
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _storeContext = storeContext;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _customerSettings = customerSettings;
    }


    //[ValidateCaptcha]
    [HttpGet]
    [Route("SubscribeNewsletter", Name = "SubscribeNewsletter")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SubscribeNewsletter([FromQuery] string email, [FromQuery] bool subscribe, bool captchaValid)
    {
        string result = string.Empty;
        var success = false;

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnNewsletterPage && !captchaValid)
        {
            result = await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage");
            return Ok(new { Success = success, Result = result, });
        }

        if (!CommonHelper.IsValidEmail(email))
        {
            result = await _localizationService.GetResourceAsync("Newsletter.Email.Wrong");
        }
        else
        {
            email = email.Trim();
            var store = await _storeContext.GetCurrentStoreAsync();
            var subscriptions = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionsByEmailAsync(email, store.Id);
            var currentLanguage = await _workContext.GetWorkingLanguageAsync();

            foreach(var subscription in subscriptions)
            {
                if (subscription != null)
                {
                    subscription.LanguageId = subscription.LanguageId == 0 ? currentLanguage.Id : subscription.LanguageId;
                    if (subscribe)
                    {
                        if (!subscription.Active)
                        {
                            await _workflowMessageService.SendNewsLetterSubscriptionActivationMessageAsync(subscription);
                        }
                        result = await _localizationService.GetResourceAsync("Newsletter.SubscribeEmailSent");
                    }
                    else
                    {
                        if (subscription.Active)
                        {
                            await _workflowMessageService.SendNewsLetterSubscriptionDeactivationMessageAsync(subscription);
                        }
                        result = await _localizationService.GetResourceAsync("Newsletter.UnsubscribeEmailSent");
                    }
                }
                else if (subscribe)
                {
                    var newSubscription = new NewsLetterSubscription
                    {
                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = false,
                        StoreId = store.Id,
                        LanguageId = currentLanguage.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(newSubscription);
                    await _workflowMessageService.SendNewsLetterSubscriptionActivationMessageAsync(newSubscription);

                    result = await _localizationService.GetResourceAsync("Newsletter.SubscribeEmailSent");
                }
                else
                {
                    result = await _localizationService.GetResourceAsync("Newsletter.UnsubscribeEmailSent");
                }
            }

            success = true;
        }

        return Ok(new
        {
            Success = success,
            Result = result,
        });
    }

    //available even when a store is closed
    [CheckAccessClosedStore(ignore: true)]

    [HttpGet]
    [Route("SubscriptionActivation", Name = "SubscriptionActivation")]
    [ProducesResponseType(typeof(SubscriptionActivationDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> SubscriptionActivation([FromQuery] Guid token, [FromQuery] bool active)
    {
        var subscriptions = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionsByGuidAsync(token);
        if (!subscriptions.Any())
            return NotFound();

        foreach (var subscription in subscriptions)
        {
            if (active)
            {
                subscription.Active = true;
                await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(subscription);

                if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()) &&
                    await _languageService.GetLanguageByIdAsync(subscription.LanguageId) is Language language)
                {
                    await _workContext.SetWorkingLanguageAsync(language);
                }
            }
            else
                await _newsLetterSubscriptionService.DeleteNewsLetterSubscriptionAsync(subscription);
        }



        var model = await _newsletterModelFactory.PrepareSubscriptionActivationDtoAsync(active);
        return Ok(model);
    }

    #region Components

    [HttpGet]
    [Route("GetNewsletterBox", Name = "GetNewsletterBox")]
    [ProducesResponseType(typeof(NewsletterBoxDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetNewsletterBox()
    {
        if (_customerSettings.HideNewsletterBlock)
            return Content("");

        var model = await _newsletterModelFactory.PrepareNewsletterBoxDtoAsync();
        return Ok(model);
    }

    #endregion




}
