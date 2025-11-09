using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Services.Customers;
using Nop.Services.Forums;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Api.Factories;
using Nop.Api.Framework.Controllers;
using Nop.Api.DTOs.PrivateMessages;
using System.Net;
using Nop.Api.DTOs.Catalog;

namespace Nop.Api.Controllers;

public partial class PrivateMessagesController : BasePublicController
{
    #region Fields

    protected readonly ForumSettings _forumSettings;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IForumService _forumService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPrivateMessagesDtoFactory _privateMessagesModelFactory;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public PrivateMessagesController(ForumSettings forumSettings,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IForumService forumService,
        ILocalizationService localizationService,
        IPrivateMessagesDtoFactory privateMessagesModelFactory,
        IStoreContext storeContext,
        IWorkContext workContext)
    {
        _forumSettings = forumSettings;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _forumService = forumService;
        _localizationService = localizationService;
        _privateMessagesModelFactory = privateMessagesModelFactory;
        _storeContext = storeContext;
        _workContext = workContext;
    }

    #endregion

    #region Methods
    [HttpGet]
    [Route("Index", Name = "PrivateMessages")]
    [ProducesResponseType(typeof(PrivateMessageIndexDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Index([FromQuery] int? pageNumber, [FromQuery] string tab)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return Error(errorMessage: "Disabled from settings");
        }

        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
        {
            return Challenge();
        }

        var model = await _privateMessagesModelFactory.PreparePrivateMessageIndexDtoAsync(pageNumber, tab);
        return Ok(model);
    }

    [HttpPost]
    [Route("DeleteInboxPM", Name = "DeleteInboxPM")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> DeleteInboxPM(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("pm", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.ToCustomerId == customer.Id)
                        {
                            pm.IsDeletedByRecipient = true;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return Ok(); // RedirectToRoute("PrivateMessages");
    }


    [HttpPost]
    [Route("MarkUnread", Name = "MarkUnread")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> MarkUnread(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("pm", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.ToCustomerId == customer.Id)
                        {
                            pm.IsRead = false;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return Ok(); // RedirectToRoute("PrivateMessages");
    }

    //updates sent items (deletes PrivateMessages)

    [HttpPost]
    [Route("DeleteSentPM", Name = "DeleteSentPM")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> DeleteSentPM(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("si", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("si", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.FromCustomerId == customer.Id)
                        {
                            pm.IsDeletedByAuthor = true;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return Ok(); // RedirectToRoute("PrivateMessages", new { tab = "sent" });
    }


    [HttpGet]
    [Route("SendPM/{toCustomerId}", Name = "SendPM")]
    [ProducesResponseType(typeof(SendPrivateMessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> SendPM([FromRoute] int toCustomerId, [FromQuery] int? replyToMessageId)
    {
        if (!_forumSettings.AllowPrivateMessages)
            return Error(errorMessage: "Disabled from settings");

        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var customerTo = await _customerService.GetCustomerByIdAsync(toCustomerId);
        if (customerTo == null || await _customerService.IsGuestAsync(customerTo))
            return NotFound(); // RedirectToRoute("PrivateMessages");

        PrivateMessage replyToPM = null;
        if (replyToMessageId.HasValue)
        {
            //reply to a previous PM
            replyToPM = await _forumService.GetPrivateMessageByIdAsync(replyToMessageId.Value);
        }

        var model = await _privateMessagesModelFactory.PrepareSendPrivateMessageDtoAsync(customerTo, replyToPM);
        return Ok(model);
    }

    [HttpPost]
    [Route("SendPM", Name = "SendPMX")]
    [ProducesResponseType(typeof(SendPrivateMessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> SendPM(SendPrivateMessageDto model)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return Error(errorMessage: "Disabled from settings");
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }

        Customer toCustomer;
        var replyToPM = await _forumService.GetPrivateMessageByIdAsync(model.ReplyToMessageId);
        if (replyToPM != null)
        {
            //reply to a previous PM
            if (replyToPM.ToCustomerId == customer.Id || replyToPM.FromCustomerId == customer.Id)
            {
                //Reply to already sent PM (by current customer) should not be sent to yourself
                toCustomer = await _customerService.GetCustomerByIdAsync(replyToPM.FromCustomerId == customer.Id
                    ? replyToPM.ToCustomerId
                    : replyToPM.FromCustomerId);
            }
            else
            {
                return Error(); // RedirectToRoute("PrivateMessages");
            }
        }
        else
        {
            //first PM
            toCustomer = await _customerService.GetCustomerByIdAsync(model.ToCustomerId);
        }

        if (toCustomer == null || await _customerService.IsGuestAsync(toCustomer))
        {
            return Error(); // RedirectToRoute("PrivateMessages");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var subject = model.Subject;
                if (_forumSettings.PMSubjectMaxLength > 0 && subject.Length > _forumSettings.PMSubjectMaxLength)
                {
                    subject = subject[0.._forumSettings.PMSubjectMaxLength];
                }

                var text = model.Message;
                if (_forumSettings.PMTextMaxLength > 0 && text.Length > _forumSettings.PMTextMaxLength)
                {
                    text = text[0.._forumSettings.PMTextMaxLength];
                }

                var nowUtc = DateTime.UtcNow;
                var store = await _storeContext.GetCurrentStoreAsync();

                var privateMessage = new PrivateMessage
                {
                    StoreId = store.Id,
                    ToCustomerId = toCustomer.Id,
                    FromCustomerId = customer.Id,
                    Subject = subject,
                    Text = text,
                    IsDeletedByAuthor = false,
                    IsDeletedByRecipient = false,
                    IsRead = false,
                    CreatedOnUtc = nowUtc
                };

                await _forumService.InsertPrivateMessageAsync(privateMessage);

                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.SendPM",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.SendPM"), toCustomer.Email), toCustomer);

                return Ok(); // RedirectToRoute("PrivateMessages", new { tab = "sent" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        model = await _privateMessagesModelFactory.PrepareSendPrivateMessageDtoAsync(toCustomer, replyToPM);
        return Ok(model);
    }

    [HttpGet]
    [Route("ViewPM/{privateMessageId}", Name = "ViewPM")]
    [ProducesResponseType(typeof(PrivateMessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ViewPM([FromRoute] int privateMessageId)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return Error(errorMessage: "Disabled from settings");
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }

        var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
        if (pm != null)
        {
            if (pm.ToCustomerId != customer.Id && pm.FromCustomerId != customer.Id)
            {
                return Error(); // RedirectToRoute("PrivateMessages");
            }

            if (!pm.IsRead && pm.ToCustomerId == customer.Id)
            {
                pm.IsRead = true;
                await _forumService.UpdatePrivateMessageAsync(pm);
            }
        }
        else
        {
            return Error(); // RedirectToRoute("PrivateMessages");
        }

        var model = await _privateMessagesModelFactory.PreparePrivateMessageDtoAsync(pm);
        return Ok(model);
    }

    [HttpDelete]
    [Route("DeletePM/{privateMessageId}", Name = "DeletePM")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]

    public virtual async Task<IActionResult> DeletePM([FromRoute] int privateMessageId)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return Error(errorMessage: "Disabled from settings");
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }

        var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
        if (pm != null)
        {
            if (pm.FromCustomerId == customer.Id)
            {
                pm.IsDeletedByAuthor = true;
                await _forumService.UpdatePrivateMessageAsync(pm);
            }

            if (pm.ToCustomerId == customer.Id)
            {
                pm.IsDeletedByRecipient = true;
                await _forumService.UpdatePrivateMessageAsync(pm);
            }
        }
        return Ok(); // RedirectToRoute("PrivateMessages");
    }

    #endregion

    #region Components
    [HttpGet]
    [Route("GetPrivateMessagesInbox", Name = "GetPrivateMessagesInbox")]
    [ProducesResponseType(typeof(PrivateMessageListDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPrivateMessagesInbox(int pageNumber, string tab)
    {
        var model = await _privateMessagesModelFactory.PrepareInboxModelAsync(pageNumber, tab);
        return Ok(model);
    }

    [HttpGet]
    [Route("GetPrivateMessagesSentItems", Name = "GetPrivateMessagesSentItems")]
    [ProducesResponseType(typeof(PrivateMessageListDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPrivateMessagesSentItems(int pageNumber, string tab)
    {
        var model = await _privateMessagesModelFactory.PrepareSentModelAsync(pageNumber, tab);
        return Ok(model);
    }
    #endregion
}
