using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Security;
using Nop.Core.Events;
using Nop.Core.Rss;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.News;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Api.Factories;
using Nop.Api.Framework;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.DTOs.News;
using System.Net;

namespace Nop.Api.Controllers;

public partial class NewsController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly ILocalizationService _localizationService;
    protected readonly INewsDtoFactory _newsModelFactory;
    protected readonly INewsService _newsService;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly IPermissionService _permissionService;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly NewsSettings _newsSettings;

    #endregion

    #region Ctor

    public NewsController(CaptchaSettings captchaSettings,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IEventPublisher eventPublisher,
        ILocalizationService localizationService,
        INewsDtoFactory newsModelFactory,
        INewsService newsService,
        INopUrlHelper nopUrlHelper,
        IPermissionService permissionService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        NewsSettings newsSettings)
    {
        _captchaSettings = captchaSettings;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _eventPublisher = eventPublisher;
        _localizationService = localizationService;
        _newsModelFactory = newsModelFactory;
        _newsService = newsService;
        _nopUrlHelper = nopUrlHelper;
        _permissionService = permissionService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _newsSettings = newsSettings;
    }

    #endregion

    #region Methods

    [HttpPost]
    [Route("List", Name = "NewsList")]
    [ProducesResponseType(typeof(NewsItemListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> List(NewsPagingFilteringDto command)
    {
        if (!_newsSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _newsModelFactory.PrepareNewsItemListDtoAsync(command);
        return Ok(model);
    }

    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("ListRss/{languageId}", Name = "NewsListRss")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ListRss([FromRoute] int languageId)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var feed = new RssFeed(
            $"{await _localizationService.GetLocalizedAsync(store, x => x.Name)}: News",
            "News",
            new Uri(_webHelper.GetStoreLocation()),
            DateTime.UtcNow);

        if (!_newsSettings.Enabled)
            return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));

        var items = new List<RssItem>();
        var newsItems = await _newsService.GetAllNewsAsync(languageId, store.Id);
        foreach (var n in newsItems)
        {
            var seName = await _urlRecordService.GetSeNameAsync(n, n.LanguageId, ensureTwoPublishedLanguages: false);
            var newsUrl = await _nopUrlHelper.RouteGenericUrlAsync<NewsItem>(new { SeName = seName }, _webHelper.GetCurrentRequestProtocol());
            items.Add(new RssItem(n.Title, n.Short, new Uri(newsUrl), $"urn:store:{store.Id}:news:blog:{n.Id}", n.CreatedOnUtc));
        }
        feed.Items = items;
        return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
    }

    [HttpGet]
    [Route("GetNewsItem/{newsItemId}", Name = "GetNewsItem")]
    [ProducesResponseType(typeof(NewsItemDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> NewsItem([FromRoute] int newsItemId)
    {
        if (!_newsSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var newsItem = await _newsService.GetNewsByIdAsync(newsItemId);
        if (newsItem == null)
            return InvokeHttp404();

        var notAvailable =
            //published?
            !newsItem.Published ||
            //availability dates
            !_newsService.IsNewsAvailable(newsItem) ||
            //Store mapping
            !await _storeMappingService.AuthorizeAsync(newsItem);
        //Check whether the current user has a "Manage news" permission (usually a store owner)
        //We should allows him (her) to use "Preview" functionality
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ContentManagement.NEWS_COMMENTS_CREATE_EDIT_DELETE);
        if (notAvailable && !hasAdminAccess)
            return InvokeHttp404();

        var model = new NewsItemDto();
        model = await _newsModelFactory.PrepareNewsItemDtoAsync(model, newsItem, true);

        ////display "edit" (manage) link
        //if (hasAdminAccess)
        //    DisplayEditLink(Url.Action("NewsItemEdit", "News", new { id = newsItem.Id, area = AreaNames.ADMIN }));

        return Ok(model);
    }


    //[ValidateCaptcha]
    [HttpPost]
    [Route("NewsCommentAdd/{newsItemId}", Name = "NewsCommentAdd")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> NewsCommentAdd([FromRoute] int newsItemId, NewsItemDto model, bool captchaValid)
    {
        if (!_newsSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var newsItem = await _newsService.GetNewsByIdAsync(newsItemId);
        if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
            return Error();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !_newsSettings.AllowNotRegisteredUsersToLeaveComments)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("News.Comments.OnlyRegisteredUsersLeaveComments"));
        }

        if (ModelState.IsValid)
        {
            var store = await _storeContext.GetCurrentStoreAsync();

            var comment = new NewsComment
            {
                NewsItemId = newsItem.Id,
                CustomerId = customer.Id,
                CommentTitle = model.AddNewComment.CommentTitle,
                CommentText = model.AddNewComment.CommentText,
                IsApproved = !_newsSettings.NewsCommentsMustBeApproved,
                StoreId = store.Id,
                CreatedOnUtc = DateTime.UtcNow,
            };

            await _newsService.InsertNewsCommentAsync(comment);

            //notify a store owner;
            if (_newsSettings.NotifyAboutNewNewsComments)
                await _workflowMessageService.SendNewsCommentStoreOwnerNotificationMessageAsync(comment, _localizationSettings.DefaultAdminLanguageId);

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.AddNewsComment",
                await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddNewsComment"), comment);

            //raise event
            if (comment.IsApproved)
                await _eventPublisher.PublishAsync(new NewsCommentApprovedEvent(comment));

            //The text boxes should be cleared after a comment has been posted
            //That' why we reload the page
            //TempData["nop.news.addcomment.result"] = comment.IsApproved
                //? await _localizationService.GetResourceAsync("News.Comments.SuccessfullyAdded")
                //: await _localizationService.GetResourceAsync("News.Comments.SeeAfterApproving");

            var seName = await _urlRecordService.GetSeNameAsync(newsItem, newsItem.LanguageId, ensureTwoPublishedLanguages: false);
            var newsUrl = await _nopUrlHelper.RouteGenericUrlAsync<NewsItem>(new { SeName = seName });
            return LocalRedirect(newsUrl);
        }

        //If we got this far, something failed, redisplay form
        RouteData.Values["action"] = "NewsItem";
        model = await _newsModelFactory.PrepareNewsItemDtoAsync(model, newsItem, true);
        return Ok(model);
    }

    #endregion
}
