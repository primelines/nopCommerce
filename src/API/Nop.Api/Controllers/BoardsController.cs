using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Security;
using Nop.Core.Rss;
using Nop.Services.Customers;
using Nop.Services.Forums;
using Nop.Services.Localization;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.DTOs.Boards;
using System.Net;
using Nop.Api.DTOs.Responses;
using Nop.Api.DTOs.Newsletter;

namespace Nop.Api.Controllers;

public partial class BoardsController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly ForumSettings _forumSettings;
    protected readonly ICustomerService _customerService;
    protected readonly IForumDtoFactory _forumModelFactory;
    protected readonly IForumService _forumService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public BoardsController(CaptchaSettings captchaSettings,
        ForumSettings forumSettings,
        ICustomerService customerService,
        IForumDtoFactory forumModelFactory,
        IForumService forumService,
        ILocalizationService localizationService,
        IStoreContext storeContext,
        IWebHelper webHelper,
        IWorkContext workContext)
    {
        _captchaSettings = captchaSettings;
        _forumSettings = forumSettings;
        _customerService = customerService;
        _forumModelFactory = forumModelFactory;
        _forumService = forumService;
        _localizationService = localizationService;
        _storeContext = storeContext;
        _webHelper = webHelper;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    [HttpGet]
    [Route("Boards", Name = "Boards")]
    [ProducesResponseType(typeof(BoardsIndexDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Index()
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _forumModelFactory.PrepareBoardsIndexDtoAsync();

        return Ok(model);
    }

    [HttpGet]
    [Route("ActiveDiscussions", Name = "ActiveDiscussions")]
    [ProducesResponseType(typeof(ActiveDiscussionsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ActiveDiscussions([FromQuery] int forumId = 0, [FromQuery] int pageNumber = 1)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _forumModelFactory.PrepareActiveDiscussionsDtoAsync(forumId, pageNumber);

        return Ok(model);
    }

    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("ActiveDiscussionsRss", Name = "ActiveDiscussionsRss")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ActiveDiscussionsRss([FromQuery] int forumId = 0)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        if (!_forumSettings.ActiveDiscussionsFeedEnabled)
            return Error(errorMessage: "Disabled from settings");

        var topics = await _forumService.GetActiveTopicsAsync(forumId, 0, _forumSettings.ActiveDiscussionsFeedCount);
        var url = Url.RouteUrl("ActiveDiscussionsRSS", null, _webHelper.GetCurrentRequestProtocol());

        var feedTitle = await _localizationService.GetResourceAsync("Forum.ActiveDiscussionsFeedTitle");
        var feedDescription = await _localizationService.GetResourceAsync("Forum.ActiveDiscussionsFeedDescription");

        var store = await _storeContext.GetCurrentStoreAsync();
        var feed = new RssFeed(
            string.Format(feedTitle, await _localizationService.GetLocalizedAsync(store, x => x.Name)),
            feedDescription,
            new Uri(url),
            DateTime.UtcNow);

        var items = new List<RssItem>();

        var viewsText = await _localizationService.GetResourceAsync("Forum.Views");
        var repliesText = await _localizationService.GetResourceAsync("Forum.Replies");

        foreach (var topic in topics)
        {
            var topicUrl = Url.RouteUrl("TopicSlug", new { id = topic.Id, slug = await _forumService.GetTopicSeNameAsync(topic) }, _webHelper.GetCurrentRequestProtocol());
            var content = $"{repliesText}: {(topic.NumPosts > 0 ? topic.NumPosts - 1 : 0)}, {viewsText}: {topic.Views}";

            items.Add(new RssItem(topic.Subject, content, new Uri(topicUrl),
                $"urn:store:{store.Id}:activeDiscussions:topic:{topic.Id}", topic.LastPostTime ?? topic.UpdatedOnUtc));
        }
        feed.Items = items;

        return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
    }


    [HttpGet]
    [Route("ForumGroup/{id}", Name = "ForumGroup")]
    [ProducesResponseType(typeof(ForumGroupDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ForumGroup([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumGroup = await _forumService.GetForumGroupByIdAsync(id);
        if (forumGroup == null)
            return NotFound();

        var model = await _forumModelFactory.PrepareForumGroupDtoAsync(forumGroup);

        return Ok(model);
    }

    [HttpGet]
    [Route("Forum/{id}", Name = "Forum")]
    [ProducesResponseType(typeof(ForumPageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Forum([FromRoute] int id, [FromQuery] int pageNumber = 1)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forum = await _forumService.GetForumByIdAsync(id);
        if (forum == null)
            return NotFound();

        var model = await _forumModelFactory.PrepareForumPageDtoAsync(forum, pageNumber);

        return Ok(model);
    }

    [HttpGet]
    [Route("ForumRss/{id}", Name = "ForumRss")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ForumRss([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        if (!_forumSettings.ForumFeedsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var topicLimit = _forumSettings.ForumFeedCount;
        var forum = await _forumService.GetForumByIdAsync(id);

        if (forum != null)
        {
            //Order by newest topic posts & limit the number of topics to return
            var topics = await _forumService.GetAllTopicsAsync(forum.Id, 0, string.Empty,
                ForumSearchType.All, 0, 0, topicLimit);

            var url = Url.RouteUrl("ForumRSS", new { id = forum.Id }, _webHelper.GetCurrentRequestProtocol());

            var feedTitle = await _localizationService.GetResourceAsync("Forum.ForumFeedTitle");
            var feedDescription = await _localizationService.GetResourceAsync("Forum.ForumFeedDescription");

            var store = await _storeContext.GetCurrentStoreAsync();
            var feed = new RssFeed(
                string.Format(feedTitle, await _localizationService.GetLocalizedAsync(store, x => x.Name), forum.Name),
                feedDescription,
                new Uri(url),
                DateTime.UtcNow);

            var items = new List<RssItem>();

            var viewsText = await _localizationService.GetResourceAsync("Forum.Views");
            var repliesText = await _localizationService.GetResourceAsync("Forum.Replies");

            foreach (var topic in topics)
            {
                var topicUrl = Url.RouteUrl("TopicSlug", new { id = topic.Id, slug = await _forumService.GetTopicSeNameAsync(topic) }, _webHelper.GetCurrentRequestProtocol());
                var content = $"{repliesText}: {(topic.NumPosts > 0 ? topic.NumPosts - 1 : 0)}, {viewsText}: {topic.Views}";

                items.Add(new RssItem(topic.Subject, content, new Uri(topicUrl), $"urn:store:{store.Id}:forum:topic:{topic.Id}", topic.LastPostTime ?? topic.UpdatedOnUtc));
            }

            feed.Items = items;

            return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
        }

        return new RssActionResult(new RssFeed(new Uri(_webHelper.GetStoreLocation())), _webHelper.GetThisPageUrl(false));
    }


    [HttpGet]
    [Route("ForumWatch/{id}", Name = "ForumWatch")]
    [ProducesResponseType(typeof(ForumWatchResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ForumWatch([FromRoute] int id)
    {
        var watchTopic = await _localizationService.GetResourceAsync("Forum.WatchForum");
        var unwatchTopic = await _localizationService.GetResourceAsync("Forum.UnwatchForum");
        var returnText = watchTopic;

        var forum = await _forumService.GetForumByIdAsync(id);
        if (forum == null)
            return Ok(new { Subscribed = false, Text = returnText, Error = true });

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
            return Ok(new { Subscribed = false, Text = returnText, Error = true });

        var forumSubscription = (await _forumService.GetAllSubscriptionsAsync(customer.Id,
            forum.Id, 0, 0, 1)).FirstOrDefault();

        bool subscribed;
        if (forumSubscription == null)
        {
            forumSubscription = new ForumSubscription
            {
                SubscriptionGuid = Guid.NewGuid(),
                CustomerId = customer.Id,
                ForumId = forum.Id,
                CreatedOnUtc = DateTime.UtcNow
            };
            await _forumService.InsertSubscriptionAsync(forumSubscription);
            subscribed = true;
            returnText = unwatchTopic;
        }
        else
        {
            await _forumService.DeleteSubscriptionAsync(forumSubscription);
            subscribed = false;
        }

        return Ok(new { Subscribed = subscribed, Text = returnText, Error = false });
    }

    [HttpGet]
    [Route("Topic/{id}", Name = "Topic")]
    [ProducesResponseType(typeof(ForumTopicPageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Topic([FromRoute] int id, [FromQuery] int pageNumber = 1)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic == null)
            return NotFound();

        var model = await _forumModelFactory.PrepareForumTopicPageDtoAsync(forumTopic, pageNumber);
        //if no posts loaded, redirect to the first page
        if (!model.ForumPosts.Any() && pageNumber > 1)
            return NotFound(); // RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });

        //update view count
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!customer.IsSearchEngineAccount())
        {
            forumTopic.Views += 1;
            await _forumService.UpdateTopicAsync(forumTopic);
        }

        return Ok(model);
    }

    [HttpGet]
    [Route("TopicWatch/{id}", Name = "TopicWatch")]
    [ProducesResponseType(typeof(TopicWatchResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> TopicWatch([FromRoute] int id)
    {
        var watchTopic = await _localizationService.GetResourceAsync("Forum.WatchTopic");
        var unwatchTopic = await _localizationService.GetResourceAsync("Forum.UnwatchTopic");
        var returnText = watchTopic;

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic == null)
            return Ok(new { Subscribed = false, Text = returnText, Error = true });

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
            return Ok(new { Subscribed = false, Text = returnText, Error = true });

        var forumSubscription = (await _forumService.GetAllSubscriptionsAsync(customer.Id,
            0, forumTopic.Id, 0, 1)).FirstOrDefault();

        bool subscribed;
        if (forumSubscription == null)
        {
            forumSubscription = new ForumSubscription
            {
                SubscriptionGuid = Guid.NewGuid(),
                CustomerId = customer.Id,
                TopicId = forumTopic.Id,
                CreatedOnUtc = DateTime.UtcNow
            };
            await _forumService.InsertSubscriptionAsync(forumSubscription);
            subscribed = true;
            returnText = unwatchTopic;
        }
        else
        {
            await _forumService.DeleteSubscriptionAsync(forumSubscription);
            subscribed = false;
        }

        return Ok(new { Subscribed = subscribed, Text = returnText, Error = false });
    }

    [HttpGet]
    [Route("TopicMove/{id}", Name = "TopicMove")]
    [ProducesResponseType(typeof(TopicMoveDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicMove([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic == null)
            return NotFound();

        var model = await _forumModelFactory.PrepareTopicMoveAsync(forumTopic);

        return Ok(model);
    }

    [HttpPost]
    [Route("TopicMove", Name = "TopicMoveX")]
    [ProducesResponseType(typeof(TopicMoveDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicMove(TopicMoveDto model)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(model.Id);

        if (forumTopic == null)
            return NotFound();

        var newForumId = model.ForumSelected;
        var forum = await _forumService.GetForumByIdAsync(newForumId);

        if (forum != null && forumTopic.ForumId != newForumId)
            await _forumService.MoveTopicAsync(forumTopic.Id, newForumId);

        return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });
    }


    [HttpDelete]
    [Route("TopicDelete/{id}", Name = "TopicDelete")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicDelete([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Ok(new
            {
                redirect = Url.RouteUrl("Homepage"),
            });

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic != null)
        {
            if (!await _forumService.IsCustomerAllowedToDeleteTopicAsync(await _workContext.GetCurrentCustomerAsync(), forumTopic))
                return Challenge();

            var forum = await _forumService.GetForumByIdAsync(forumTopic.ForumId);

            await _forumService.DeleteTopicAsync(forumTopic);

            if (forum != null)
                return Ok(new
                {
                    redirect = Url.RouteUrl("ForumSlug", new { id = forum.Id, slug = await _forumService.GetForumSeNameAsync(forum) }),
                });
        }

        return Ok(new
        {
            redirect = Url.RouteUrl("Boards"),
        });
    }

    [HttpGet]
    [Route("TopicCreate/{id}", Name = "TopicCreate")]
    [ProducesResponseType(typeof(EditForumTopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicCreate([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forum = await _forumService.GetForumByIdAsync(id);
        if (forum == null)
            return NotFound();

        if (await _forumService.IsCustomerAllowedToCreateTopicAsync(await _workContext.GetCurrentCustomerAsync(), forum) == false)
            return Challenge();

        var model = new EditForumTopicDto();
        await _forumModelFactory.PrepareTopicCreateModelAsync(forum, model);
        return Ok(model);
    }

    //[ValidateCaptcha]
    [HttpPost]
    [Route("TopicCreate", Name = "TopicCreateX")]
    [ProducesResponseType(typeof(EditForumTopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicCreate(EditForumTopicDto model, bool captchaValid)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forum = await _forumService.GetForumByIdAsync(model.ForumId);
        if (forum == null)
            return NotFound();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnForum && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            try
            {
                var customer = await _workContext.GetCurrentCustomerAsync();
                if (!await _forumService.IsCustomerAllowedToCreateTopicAsync(customer, forum))
                {
                    return Challenge();
                }

                var subject = model.Subject;
                var maxSubjectLength = _forumSettings.TopicSubjectMaxLength;
                if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                {
                    subject = subject[0..maxSubjectLength];
                }

                var text = model.Text;
                var maxPostLength = _forumSettings.PostMaxLength;
                if (maxPostLength > 0 && text.Length > maxPostLength)
                    text = text[0..maxPostLength];

                var topicType = ForumTopicType.Normal;
                var ipAddress = _webHelper.GetCurrentIpAddress();
                var nowUtc = DateTime.UtcNow;

                if (await _forumService.IsCustomerAllowedToSetTopicPriorityAsync(customer))
                    topicType = (ForumTopicType)Enum.ToObject(typeof(ForumTopicType), model.TopicTypeId);

                //forum topic
                var forumTopic = new ForumTopic
                {
                    ForumId = forum.Id,
                    CustomerId = customer.Id,
                    TopicTypeId = (int)topicType,
                    Subject = subject,
                    CreatedOnUtc = nowUtc,
                    UpdatedOnUtc = nowUtc
                };
                await _forumService.InsertTopicAsync(forumTopic, true);

                //forum post
                var forumPost = new ForumPost
                {
                    TopicId = forumTopic.Id,
                    CustomerId = customer.Id,
                    Text = text,
                    IPAddress = ipAddress,
                    CreatedOnUtc = nowUtc,
                    UpdatedOnUtc = nowUtc
                };
                await _forumService.InsertPostAsync(forumPost, false);

                //update forum topic
                forumTopic.NumPosts = 1;
                forumTopic.LastPostId = forumPost.Id;
                forumTopic.LastPostCustomerId = forumPost.CustomerId;
                forumTopic.LastPostTime = forumPost.CreatedOnUtc;
                forumTopic.UpdatedOnUtc = nowUtc;
                await _forumService.UpdateTopicAsync(forumTopic);

                //subscription                
                if (await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
                {
                    if (model.Subscribed)
                    {
                        var forumSubscription = new ForumSubscription
                        {
                            SubscriptionGuid = Guid.NewGuid(),
                            CustomerId = customer.Id,
                            TopicId = forumTopic.Id,
                            CreatedOnUtc = nowUtc
                        };

                        await _forumService.InsertSubscriptionAsync(forumSubscription);
                    }
                }

                return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        //redisplay form
        await _forumModelFactory.PrepareTopicCreateModelAsync(forum, model);

        return Ok(model);
    }

    [HttpGet]
    [Route("TopicEdit/{id}", Name = "TopicEdit")]
    [ProducesResponseType(typeof(EditForumTopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicEdit([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic == null)
            return NotFound();

        if (!await _forumService.IsCustomerAllowedToEditTopicAsync(await _workContext.GetCurrentCustomerAsync(), forumTopic))
            return Challenge();

        var model = new EditForumTopicDto();
        await _forumModelFactory.PrepareTopicEditModelAsync(forumTopic, model, false);

        return Ok(model);
    }


    //[ValidateCaptcha]

    [HttpPost]
    [Route("TopicEdit", Name = "TopicEditX")]
    [ProducesResponseType(typeof(EditForumTopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicEdit(EditForumTopicDto model, bool captchaValid)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(model.Id);
        if (forumTopic == null)
            return NotFound();

        var forum = await _forumService.GetForumByIdAsync(forumTopic.ForumId);
        if (forum == null)
            return NotFound();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnForum && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            try
            {
                var customer = await _workContext.GetCurrentCustomerAsync();
                if (!await _forumService.IsCustomerAllowedToEditTopicAsync(customer, forumTopic))
                    return Challenge();

                var subject = model.Subject;
                var maxSubjectLength = _forumSettings.TopicSubjectMaxLength;
                if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                {
                    subject = subject[0..maxSubjectLength];
                }

                var text = model.Text;
                var maxPostLength = _forumSettings.PostMaxLength;
                if (maxPostLength > 0 && text.Length > maxPostLength)
                    text = text[0..maxPostLength];

                var topicType = ForumTopicType.Normal;
                var ipAddress = _webHelper.GetCurrentIpAddress();
                var nowUtc = DateTime.UtcNow;

                if (await _forumService.IsCustomerAllowedToSetTopicPriorityAsync(customer))
                    topicType = (ForumTopicType)Enum.ToObject(typeof(ForumTopicType), model.TopicTypeId);

                //forum topic
                forumTopic.TopicTypeId = (int)topicType;
                forumTopic.Subject = subject;
                forumTopic.UpdatedOnUtc = nowUtc;
                await _forumService.UpdateTopicAsync(forumTopic);

                //forum post                
                var firstPost = await _forumService.GetFirstPostAsync(forumTopic);
                if (firstPost != null)
                {
                    firstPost.Text = text;
                    firstPost.UpdatedOnUtc = nowUtc;
                    await _forumService.UpdatePostAsync(firstPost);
                }
                else
                {
                    //error (not possible)
                    firstPost = new ForumPost
                    {
                        TopicId = forumTopic.Id,
                        CustomerId = forumTopic.CustomerId,
                        Text = text,
                        IPAddress = ipAddress,
                        UpdatedOnUtc = nowUtc
                    };

                    await _forumService.InsertPostAsync(firstPost, false);
                }

                //subscription
                if (await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
                {
                    var forumSubscription = (await _forumService.GetAllSubscriptionsAsync(customer.Id,
                        0, forumTopic.Id, 0, 1)).FirstOrDefault();
                    if (model.Subscribed)
                    {
                        if (forumSubscription == null)
                        {
                            forumSubscription = new ForumSubscription
                            {
                                SubscriptionGuid = Guid.NewGuid(),
                                CustomerId = customer.Id,
                                TopicId = forumTopic.Id,
                                CreatedOnUtc = nowUtc
                            };

                            await _forumService.InsertSubscriptionAsync(forumSubscription);
                        }
                    }
                    else
                    {
                        if (forumSubscription != null)
                        {
                            await _forumService.DeleteSubscriptionAsync(forumSubscription);
                        }
                    }
                }

                // redirect to the topic page with the topic slug
                return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        //redisplay form
        await _forumModelFactory.PrepareTopicEditModelAsync(forumTopic, model, true);

        return Ok(model);
    }

    [HttpDelete]
    [Route("PostDelete/{id}", Name = "PostDelete")]
    [ProducesResponseType(typeof(EditForumTopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PostDelete([FromRoute] int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Ok(new
            {
                redirect = Url.RouteUrl("Homepage"),
            });

        var forumPost = await _forumService.GetPostByIdAsync(id);

        if (forumPost == null)
            return Ok(new { redirect = Url.RouteUrl("Boards") });

        if (!await _forumService.IsCustomerAllowedToDeletePostAsync(await _workContext.GetCurrentCustomerAsync(), forumPost))
            return Challenge();

        var forumTopic = await _forumService.GetTopicByIdAsync(forumPost.TopicId);
        var forumId = forumTopic.ForumId;
        var forum = await _forumService.GetForumByIdAsync(forumId);
        var forumSlug = await _forumService.GetForumSeNameAsync(forum);

        await _forumService.DeletePostAsync(forumPost);

        //get topic one more time because it can be deleted (first or only post deleted)
        forumTopic = await _forumService.GetTopicByIdAsync(forumPost.TopicId);
        if (forumTopic == null)
            return Ok(new
            {
                redirect = Url.RouteUrl("ForumSlug", new { id = forumId, slug = forumSlug }),
            });

        return Ok(new
        {
            redirect = Url.RouteUrl("TopicSlug", new { id = forumTopic.Id, slug = await _forumService.GetTopicSeNameAsync(forumTopic) }),
        });

    }

    //TODO: parameter are defrent , needs review
    //[HttpGet]
    //[Route("PostCreate/{id}/{quote}", Name = "{id}")]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    //([FromRoute] id[FromQuery] quoteId[FromRoute]  quote)
    [HttpGet]
    [Route("PostCreate/{id}/{quote}", Name = "PostCreate")]
    [ProducesResponseType(typeof(EditForumPostDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PostCreate([FromRoute] int id, [FromRoute] int? quote)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(id);
        if (forumTopic == null)
            return NotFound();

        if (!await _forumService.IsCustomerAllowedToCreatePostAsync(await _workContext.GetCurrentCustomerAsync(), forumTopic))
            return Challenge();

        var model = await _forumModelFactory.PreparePostCreateModelAsync(forumTopic, quote, false);

        return Ok(model);
    }


    //[ValidateCaptcha]

    [HttpPost]
    [Route("PostCreate", Name = "PostCreateX")]
    [ProducesResponseType(typeof(EditForumPostDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PostCreate(EditForumPostDto model, bool captchaValid)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumTopic = await _forumService.GetTopicByIdAsync(model.ForumTopicId);
        if (forumTopic == null)
            return NotFound();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnForum && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            try
            {
                var customer = await _workContext.GetCurrentCustomerAsync();
                if (!await _forumService.IsCustomerAllowedToCreatePostAsync(customer, forumTopic))
                    return Challenge();

                var text = model.Text;
                var maxPostLength = _forumSettings.PostMaxLength;
                if (maxPostLength > 0 && text.Length > maxPostLength)
                    text = text[0..maxPostLength];

                var ipAddress = _webHelper.GetCurrentIpAddress();

                var nowUtc = DateTime.UtcNow;

                var forumPost = new ForumPost
                {
                    TopicId = forumTopic.Id,
                    CustomerId = customer.Id,
                    Text = text,
                    IPAddress = ipAddress,
                    CreatedOnUtc = nowUtc,
                    UpdatedOnUtc = nowUtc
                };
                await _forumService.InsertPostAsync(forumPost, true);

                //subscription
                if (await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
                {
                    var forumSubscription = (await _forumService.GetAllSubscriptionsAsync(customer.Id,
                        0, forumPost.TopicId, 0, 1)).FirstOrDefault();
                    if (model.Subscribed)
                    {
                        if (forumSubscription == null)
                        {
                            forumSubscription = new ForumSubscription
                            {
                                SubscriptionGuid = Guid.NewGuid(),
                                CustomerId = customer.Id,
                                TopicId = forumPost.TopicId,
                                CreatedOnUtc = nowUtc
                            };

                            await _forumService.InsertSubscriptionAsync(forumSubscription);
                        }
                    }
                    else
                    {
                        if (forumSubscription != null)
                        {
                            await _forumService.DeleteSubscriptionAsync(forumSubscription);
                        }
                    }
                }

                var pageSize = _forumSettings.PostsPageSize > 0 ? _forumSettings.PostsPageSize : 10;

                var pageIndex = await _forumService.CalculateTopicPageIndexAsync(forumPost.TopicId, pageSize, forumPost.Id) + 1;
                string url;
                if (pageIndex > 1)
                    url = Url.RouteUrl("TopicSlugPaged", new { id = forumPost.TopicId, slug = await _forumService.GetTopicSeNameAsync(forumTopic), pageNumber = pageIndex });
                else
                    url = Url.RouteUrl("TopicSlug", new { id = forumPost.TopicId, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });
                return LocalRedirect($"{url}#{forumPost.Id}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        //redisplay form
        model = await _forumModelFactory.PreparePostCreateModelAsync(forumTopic, 0, true);

        return Ok(model);
    }

    [HttpGet]
    [Route("PostEdit/{id}", Name = "PostEdit")]
    [ProducesResponseType(typeof(EditForumPostDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PostEdit([FromRoute]  int id)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumPost = await _forumService.GetPostByIdAsync(id);
        if (forumPost == null)
            return NotFound();

        if (!await _forumService.IsCustomerAllowedToEditPostAsync(await _workContext.GetCurrentCustomerAsync(), forumPost))
            return Challenge();

        var model = await _forumModelFactory.PreparePostEditModelAsync(forumPost, false);

        return Ok(model);
    }

    //[ValidateCaptcha]

    [HttpPost]
    [Route("PostEdit", Name = "PostEditX")]
    [ProducesResponseType(typeof(EditForumPostDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> PostEdit(EditForumPostDto model, bool captchaValid)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var forumPost = await _forumService.GetPostByIdAsync(model.Id);
        if (forumPost == null)
            return NotFound();

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _forumService.IsCustomerAllowedToEditPostAsync(customer, forumPost))
            return Challenge();

        var forumTopic = await _forumService.GetTopicByIdAsync(forumPost.TopicId);
        if (forumTopic == null)
            return NotFound();

        var forum = await _forumService.GetForumByIdAsync(forumTopic.ForumId);
        if (forum == null)
            return NotFound();

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnForum && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        if (ModelState.IsValid)
        {
            try
            {
                var nowUtc = DateTime.UtcNow;

                var text = model.Text;
                var maxPostLength = _forumSettings.PostMaxLength;
                if (maxPostLength > 0 && text.Length > maxPostLength)
                {
                    text = text[0..maxPostLength];
                }

                forumPost.UpdatedOnUtc = nowUtc;
                forumPost.Text = text;
                await _forumService.UpdatePostAsync(forumPost);

                //subscription
                if (await _forumService.IsCustomerAllowedToSubscribeAsync(customer))
                {
                    var forumSubscription = (await _forumService.GetAllSubscriptionsAsync(customer.Id,
                        0, forumPost.TopicId, 0, 1)).FirstOrDefault();
                    if (model.Subscribed)
                    {
                        if (forumSubscription == null)
                        {
                            forumSubscription = new ForumSubscription
                            {
                                SubscriptionGuid = Guid.NewGuid(),
                                CustomerId = customer.Id,
                                TopicId = forumPost.TopicId,
                                CreatedOnUtc = nowUtc
                            };
                            await _forumService.InsertSubscriptionAsync(forumSubscription);
                        }
                    }
                    else
                    {
                        if (forumSubscription != null)
                        {
                            await _forumService.DeleteSubscriptionAsync(forumSubscription);
                        }
                    }
                }

                var pageSize = _forumSettings.PostsPageSize > 0 ? _forumSettings.PostsPageSize : 10;
                var pageIndex = (await _forumService.CalculateTopicPageIndexAsync(forumPost.TopicId, pageSize, forumPost.Id) + 1);
                string url;
                if (pageIndex > 1)
                {
                    url = Url.RouteUrl("TopicSlugPaged", new { id = forumPost.TopicId, slug = await _forumService.GetTopicSeNameAsync(forumTopic), pageNumber = pageIndex });
                }
                else
                {
                    url = Url.RouteUrl("TopicSlug", new { id = forumPost.TopicId, slug = await _forumService.GetTopicSeNameAsync(forumTopic) });
                }
                return LocalRedirect($"{url}#{forumPost.Id}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        //redisplay form
        model = await _forumModelFactory.PreparePostEditModelAsync(forumPost, true);

        return Ok(model);
    }

    [HttpGet]
    [Route("Search/{forumId}", Name = "BoardsSearch")]
    [ProducesResponseType(typeof(BoardsSearchDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Search([FromQuery] string searchterms, [FromQuery] bool? advs, [FromRoute] string forumId,
        [FromQuery] string within, [FromQuery] string limitDays, [FromQuery] int pageNumber = 1)
    {
        if (!_forumSettings.ForumsEnabled)
            return Error(errorMessage: "Disabled from settings");

        var model = await _forumModelFactory.PrepareSearchDtoAsync(searchterms, advs, forumId, within, limitDays, pageNumber);

        return Ok(model);
    }

    //[HttpGet]
    //[Route("CustomerForumSubscriptions", Name = "CustomerForumSubscriptions")]
    //[ProducesResponseType(typeof(CustomerForumSubscriptionsDto), (int)HttpStatusCode.OK)]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    //public virtual async Task<IActionResult> CustomerForumSubscriptions([FromQuery] int? pageNumber)
    //{
    //    if (!_forumSettings.AllowCustomersToManageSubscriptions)
    //        return RedirectToRoute("CustomerInfo");

    //    var model = await _forumModelFactory.PrepareCustomerForumSubscriptionsDtoAsync(pageNumber);

    //    return Ok(model);
    //}

    [HttpPost]
    [Route("CustomerForumSubscriptionsPOST", Name = "CustomerForumSubscriptionsPOST")]
    [ProducesResponseType(typeof(CustomerForumSubscriptionsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> CustomerForumSubscriptionsPOST(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("fs", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("fs", "").Trim();
                if (int.TryParse(id, out var forumSubscriptionId))
                {
                    var forumSubscription = await _forumService.GetSubscriptionByIdAsync(forumSubscriptionId);
                    var customer = await _workContext.GetCurrentCustomerAsync();

                    if (forumSubscription != null && forumSubscription.CustomerId == customer.Id)
                    {
                        await _forumService.DeleteSubscriptionAsync(forumSubscription);
                    }
                }
            }
        }

        return RedirectToRoute("CustomerForumSubscriptions");
    }

    //[HttpGet]
    //[Route("PostVote/{postId}", Name = "PostVote")]
    //[ProducesResponseType(typeof(PostVoteResponse), (int)HttpStatusCode.OK)]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    //[ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    //public virtual async Task<IActionResult> PostVote([FromRoute] int postId, [FromQuery] bool isUp)
    //{
    //    if (!_forumSettings.AllowPostVoting)
    //        return new NullJsonResult();

    //    var forumPost = await _forumService.GetPostByIdAsync(postId);
    //    if (forumPost == null)
    //        return new NullJsonResult();

    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    if (!await _customerService.IsRegisteredAsync(customer))
    //        return Ok(new
    //        {
    //            Error = await _localizationService.GetResourceAsync("Forum.Votes.Login"),
    //            VoteCount = forumPost.VoteCount
    //        });

    //    if (customer.Id == forumPost.CustomerId)
    //        return Ok(new
    //        {
    //            Error = await _localizationService.GetResourceAsync("Forum.Votes.OwnPost"),
    //            VoteCount = forumPost.VoteCount
    //        });

    //    var forumPostVote = await _forumService.GetPostVoteAsync(postId, customer);
    //    if (forumPostVote != null)
    //    {
    //        if ((forumPostVote.IsUp && isUp) || (!forumPostVote.IsUp && !isUp))
    //            return Ok(new
    //            {
    //                Error = await _localizationService.GetResourceAsync("Forum.Votes.AlreadyVoted"),
    //                VoteCount = forumPost.VoteCount
    //            });

    //        await _forumService.DeletePostVoteAsync(forumPostVote);
    //        return Ok(new { VoteCount = forumPost.VoteCount });
    //    }

    //    if (await _forumService.GetNumberOfPostVotesAsync(customer, DateTime.UtcNow.AddDays(-1)) >= _forumSettings.MaxVotesPerDay)
    //        return Ok(new
    //        {
    //            Error = string.Format(await _localizationService.GetResourceAsync("Forum.Votes.MaxVotesReached"), _forumSettings.MaxVotesPerDay),
    //            VoteCount = forumPost.VoteCount
    //        });

    //    await _forumService.InsertPostVoteAsync(new ForumPostVote
    //    {
    //        CustomerId = customer.Id,
    //        ForumPostId = postId,
    //        IsUp = isUp,
    //        CreatedOnUtc = DateTime.UtcNow
    //    });

    //    return Ok(new { VoteCount = forumPost.VoteCount, IsUp = isUp });
    //}

    #endregion

    #region Components

    //[HttpGet]
    //[Route("GetForumLastPost", Name = "GetForumLastPost")]
    //[ProducesResponseType(typeof(LastPostDto), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetForumLastPost([FromRoute]int forumPostId, [FromQuery] bool showTopic)
    //{
    //    var forumPost = await _forumService.GetPostByIdAsync(forumPostId);
    //    var model = await _forumModelFactory.PrepareLastPostDtoAsync(forumPost, showTopic);

    //    return Ok(model);
    //}

    //[HttpGet]
    //[Route("GetForumBreadcrumb", Name = "GetForumBreadcrumb")]
    //[ProducesResponseType(typeof(ForumBreadcrumbDto), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetForumBreadcrumb([FromQuery] int? forumGroupId,[FromQuery] int? forumId, [FromQuery] int? forumTopicId)
    //{
    //    var model = await _forumModelFactory.PrepareForumBreadcrumbDtoAsync(forumGroupId, forumId, forumTopicId);
    //    return Ok(model);
    //}


    [HttpGet]
    [Route("GetForumActiveDiscussionsSmall", Name = "GetForumActiveDiscussionsSmall")]
    [ProducesResponseType(typeof(ActiveDiscussionsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetForumActiveDiscussionsSmall()
    {
        var model = await _forumModelFactory.PrepareActiveDiscussionsDtoAsync();
        if (!model.ForumTopics.Any())
            return Content("");

        return Ok(model);
    }

    #endregion
}
