using Microsoft.AspNetCore.Mvc;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Topics;
using Nop.Api.Factories;
using Nop.Api.Framework;
using Nop.Api.Framework.Mvc.Filters;
using System.Net;
using Nop.Api.DTOs.Topics;
using Nop.Core.Domain.Vendors;
using Nop.Api.DTOs.Boards;

namespace Nop.Api.Controllers;

public partial class TopicController : BasePublicController
{
    #region Fields

    protected readonly IAclService _aclService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly ITopicDtoFactory _topicModelFactory;
    protected readonly ITopicService _topicService;

    #endregion

    #region Ctor

    public TopicController(IAclService aclService,
        ILocalizationService localizationService,
        IPermissionService permissionService,
        IStoreMappingService storeMappingService,
        ITopicDtoFactory topicModelFactory,
        ITopicService topicService)
    {
        _aclService = aclService;
        _localizationService = localizationService;
        _permissionService = permissionService;
        _storeMappingService = storeMappingService;
        _topicModelFactory = topicModelFactory;
        _topicService = topicService;
    }

    #endregion

    #region Methods

    [HttpGet]
    [Route("GetTopicDetails/{topicId}", Name = "GetTopicDetails")]
    [ProducesResponseType(typeof(TopicDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> TopicDetails([FromRoute] int topicId)
    {
        var topic = await _topicService.GetTopicByIdAsync(topicId);

        if (topic == null)
            return InvokeHttp404();

        var notAvailable = !topic.Published ||
                           //ACL (access control list)
                           !await _aclService.AuthorizeAsync(topic) ||
                           //store mapping
                           !await _storeMappingService.AuthorizeAsync(topic);

        //allow administrators to preview any topic
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ContentManagement.TOPICS_CREATE_EDIT_DELETE);

        if (notAvailable && !hasAdminAccess)
            return InvokeHttp404();

        var model = await _topicModelFactory.PrepareTopicDtoAsync(topic);

        ////display "edit" (manage) link
        //if (hasAdminAccess)
        //    DisplayEditLink(Url.Action("Edit", "Topic", new { id = model.Id, area = AreaNames.ADMIN }));

        ////template
        //var templateViewPath = await _topicModelFactory.PrepareTemplateViewPathAsync(model.TopicTemplateId);
        return Ok(model);
    }

    //[CheckLanguageSeoCode(ignore: true)]
    ////TODO: no routs

    //public virtual async Task<IActionResult> TopicDetailsPopup(string systemName)
    //{
    //    var model = await _topicModelFactory.PrepareTopicDtoBySystemNameAsync(systemName);
    //    if (model == null)
    //        return InvokeHttp404();

    //    ////ViewBag.IsPopup = true;

    //    //template
    //    var templateViewPath = await _topicModelFactory.PrepareTemplateViewPathAsync(model.TopicTemplateId);
    //    return Ok(model);
    //}

    //[HttpPost]
    //[Route("Authenticate")]
    //TODO: no routs

    //public virtual async Task<IActionResult> Authenticate(int id, string password)
    //{
    //    var authResult = false;
    //    var title = string.Empty;
    //    var body = string.Empty;
    //    var error = string.Empty;

    //    var topic = await _topicService.GetTopicByIdAsync(id);
    //    if (topic != null &&
    //        topic.Published &&
    //        //password protected?
    //        topic.IsPasswordProtected &&
    //        //store mapping
    //        await _storeMappingService.AuthorizeAsync(topic) &&
    //        //ACL (access control list)
    //        await _aclService.AuthorizeAsync(topic))
    //    {
    //        if (topic.Password != null && topic.Password.Equals(password))
    //        {
    //            authResult = true;
    //            title = await _localizationService.GetLocalizedAsync(topic, x => x.Title);
    //            body = await _localizationService.GetLocalizedAsync(topic, x => x.Body);
    //        }
    //        else
    //        {
    //            error = await _localizationService.GetResourceAsync("Topic.WrongPassword");
    //        }
    //    }

    //    return Ok(new { Authenticated = authResult, Title = title, Body = body, Error = error });
    //}

    #endregion

    #region Components

    [HttpGet]
    [Route("GetTopicBlock", Name = "GetTopicBlock")]
    [ProducesResponseType(typeof(TopicDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTopicBlock(string systemName)
    {
        var model = await _topicModelFactory.PrepareTopicDtoBySystemNameAsync(systemName);
        if (model == null)
            return Content("");
        return Ok(model);
    }



    #endregion
}
