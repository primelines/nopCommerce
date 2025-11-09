using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Events;
using Nop.Core.Rss;
using Nop.Services.Blogs;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.DTOs.Blogs;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Nop.Api.Controllers;

[Authorize(Policy = JwtBearerDefaults.AuthenticationScheme, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public partial class BlogController : BasePublicController
{
    #region Fields

    protected readonly BlogSettings _blogSettings;
    protected readonly CaptchaSettings _captchaSettings;
    protected readonly IBlogDtoFactory _blogModelFactory;
    protected readonly IBlogService _blogService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly ILocalizationService _localizationService;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly IPermissionService _permissionService;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;

    #endregion

    #region Ctor

    public BlogController(BlogSettings blogSettings,
        CaptchaSettings captchaSettings,
        IBlogDtoFactory blogModelFactory,
        IBlogService blogService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IEventPublisher eventPublisher,
        ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        IPermissionService permissionService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings)
    {
        _blogSettings = blogSettings;
        _captchaSettings = captchaSettings;
        _blogModelFactory = blogModelFactory;
        _blogService = blogService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _eventPublisher = eventPublisher;
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _permissionService = permissionService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
    }

    #endregion

    #region Methods
    [HttpPost]
    [Route("List", Name = "BlogList")]
    [ProducesResponseType(typeof(BlogPostListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> List(BlogPagingFilteringDto command)
    {

       var customer = await _workContext.GetCurrentCustomerAsync();

        var isGeust = await _customerService.IsGuestAsync(customer);
        var isAdmin = await _customerService.IsAdminAsync(customer);
        var isVendor = await _customerService.IsVendorAsync(customer);

        if (!_blogSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _blogModelFactory.PrepareBlogPostListDtoAsync(command);
        return Ok(model);
    }

    [HttpPost]
    [Route("BlogByTag", Name = "BlogByTag")]
    [ProducesResponseType(typeof(BlogPostListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> BlogByTag(BlogPagingFilteringDto command)
    {
        if (!_blogSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _blogModelFactory.PrepareBlogPostListDtoAsync(command);
        return Ok(model);
    }

    [HttpPost]
    [Route("BlogByMonth", Name = "BlogByMonth")]
    [ProducesResponseType(typeof(BlogPostListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> BlogByMonth(BlogPagingFilteringDto command)
    {
        if (!_blogSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _blogModelFactory.PrepareBlogPostListDtoAsync(command);
        return Ok(model);
    }

    [HttpGet]
    [Route("ListRss/{languageId}", Name = "BlogListRss")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ListRss([FromRoute] int languageId)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var feed = new RssFeed(
            $"{await _localizationService.GetLocalizedAsync(store, x => x.Name)}: Blog",
            "Blog",
            new Uri(_webHelper.GetStoreLocation()),
            DateTime.UtcNow);

        if (!_blogSettings.Enabled)
            return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));

        var items = new List<RssItem>();
        var blogPosts = await _blogService.GetAllBlogPostsAsync(store.Id, languageId);
        foreach (var blogPost in blogPosts)
        {
            var seName = await _urlRecordService.GetSeNameAsync(blogPost, blogPost.LanguageId, ensureTwoPublishedLanguages: false);
            var blogPostUrl = await _nopUrlHelper.RouteGenericUrlAsync<BlogPost>(new { SeName = seName }, _webHelper.GetCurrentRequestProtocol());
            items.Add(new RssItem(blogPost.Title, blogPost.Body, new Uri(blogPostUrl), $"urn:store:{store.Id}:blog:post:{blogPost.Id}", blogPost.CreatedOnUtc));
        }
        feed.Items = items;
        return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
    }

    [HttpGet]
    [Route("GetBlogPost/{blogPostId}", Name = "GetBlogPost")]
    [ProducesResponseType(typeof(BlogPostDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> BlogPost([FromRoute] int blogPostId)
    {
        if (!_blogSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var blogPost = await _blogService.GetBlogPostByIdAsync(blogPostId);
        if (blogPost == null)
            return InvokeHttp404();

        var notAvailable =
            //availability dates
            !_blogService.BlogPostIsAvailable(blogPost) ||
            //Store mapping
            !await _storeMappingService.AuthorizeAsync(blogPost);
        //Check whether the current user has a "Manage blog" permission (usually a store owner)
        //We should allows him (her) to use "Preview" functionality
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ContentManagement.BLOG_VIEW);
        if (notAvailable && !hasAdminAccess)
            return InvokeHttp404();

        ////display "edit" (manage) link
        //if (hasAdminAccess)
        //    DisplayEditLink(Url.Action("BlogPostEdit", "Blog", new { id = blogPost.Id, area = AreaNames.ADMIN }));

        var model = new BlogPostDto();
        await _blogModelFactory.PrepareBlogPostDtoAsync(model, blogPost, true);

        return Ok(model);
    }


    //[ValidateCaptcha]

    [HttpPost]
    [Route("BlogCommentAdd/{blogPostId}", Name = "BlogCommentAdd")]
    [ProducesResponseType(typeof(BlogCommentDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> BlogCommentAdd([FromRoute] int blogPostId, AddBlogCommentDto model)
    {
        if (!_blogSettings.Enabled)
            return Error(errorMessage: "Disabled from settings");

        var blogPost = await _blogService.GetBlogPostByIdAsync(blogPostId);
        if (blogPost == null || !blogPost.AllowComments)
            return Error(errorMessage: "Blog post not found of comments not allowed");

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !_blogSettings.AllowNotRegisteredUsersToLeaveComments)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Blog.Comments.OnlyRegisteredUsersLeaveComments"));
        }


        if (ModelState.IsValid)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var comment = new BlogComment
            {
                BlogPostId = blogPost.Id,
                CustomerId = customer.Id,
                CommentText = model.CommentText,
                IsApproved = !_blogSettings.BlogCommentsMustBeApproved,
                StoreId = store.Id,
                CreatedOnUtc = DateTime.UtcNow,
            };

            await _blogService.InsertBlogCommentAsync(comment);

            //notify a store owner
            if (_blogSettings.NotifyAboutNewBlogComments)
                await _workflowMessageService.SendBlogCommentStoreOwnerNotificationMessageAsync(comment, _localizationSettings.DefaultAdminLanguageId);

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.AddBlogComment",
                await _localizationService.GetResourceAsync("ActivityLog.PublicStore.AddBlogComment"), comment);

            //raise event
            if (comment.IsApproved)
                await _eventPublisher.PublishAsync(new BlogCommentApprovedEvent(comment));

            ////The text boxes should be cleared after a comment has been posted
            ////That' why we reload the page
            //TempData["nop.blog.addcomment.result"] = comment.IsApproved
            //    ? await _localizationService.GetResourceAsync("Blog.Comments.SuccessfullyAdded")
            //    : await _localizationService.GetResourceAsync("Blog.Comments.SeeAfterApproving");

            //var seName = await _urlRecordService.GetSeNameAsync(blogPost, blogPost.LanguageId, ensureTwoPublishedLanguages: false);
            //var blogPostUrl = await _nopUrlHelper.RouteGenericUrlAsync<BlogPost>(new { SeName = seName });
            //return LocalRedirect(blogPostUrl);


            await _blogModelFactory.PrepareBlogPostCommentModelAsync(comment);
            return Ok(model);

        }

        return Error();

    }

    #endregion

    #region Components
    [HttpGet]
    [Route("GetBlogMonths", Name = "GetBlogMonths")]
    [ProducesResponseType(typeof(BlogPostYearDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBlogMonths()
    {
        if (!_blogSettings.Enabled)
            return Content("");

        var model = await _blogModelFactory.PrepareBlogPostYearModelAsync();
        return Ok(model);
    }

    [HttpGet]
    [Route("GetBlogTags", Name = "GetBlogTags")]
    [ProducesResponseType(typeof(BlogPostTagListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]

    public async Task<IActionResult> GetBlogTags()
    {
        if (!_blogSettings.Enabled)
            return Content("");

        var model = await _blogModelFactory.PrepareBlogPostTagListDtoAsync();
        return Ok(model);
    }
    #endregion
}
