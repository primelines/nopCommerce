using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Polls;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Polls;
using Nop.Services.Stores;
using Nop.Api.Factories;
using System.Net;
using Nop.Api.DTOs.Polls;
using Nop.Api.DTOs.Catalog;

namespace Nop.Api.Controllers;

public partial class PollController : BasePublicController
{
    #region Fields

    protected readonly ICustomerService _customerService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPollDtoFactory _pollModelFactory;
    protected readonly IPollService _pollService;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public PollController(ICustomerService customerService,
        ILocalizationService localizationService,
        IPollDtoFactory pollModelFactory,
        IPollService pollService,
        IStoreMappingService storeMappingService,
        IWorkContext workContext)
    {
        _customerService = customerService;
        _localizationService = localizationService;
        _pollModelFactory = pollModelFactory;
        _pollService = pollService;
        _storeMappingService = storeMappingService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    [HttpGet]
    [Route("Vote/{pollAnswerId}", Name = "Vote")]
    [ProducesResponseType(typeof(PollDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Vote([FromRoute] int pollAnswerId)
    {
        var pollAnswer = await _pollService.GetPollAnswerByIdAsync(pollAnswerId);
        if (pollAnswer == null)
            return Ok(new { error = "No poll answer found with the specified id" });

        var poll = await _pollService.GetPollByIdAsync(pollAnswer.PollId);

        if (!poll.Published || !await _storeMappingService.AuthorizeAsync(poll))
            return Ok(new { error = "Poll is not available" });

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer) && !poll.AllowGuestsToVote)
            return Ok(new { error = await _localizationService.GetResourceAsync("Polls.OnlyRegisteredUsersVote") });

        var alreadyVoted = await _pollService.AlreadyVotedAsync(poll.Id, customer.Id);
        if (!alreadyVoted)
        {
            //vote
            await _pollService.InsertPollVotingRecordAsync(new PollVotingRecord
            {
                PollAnswerId = pollAnswer.Id,
                CustomerId = customer.Id,
                CreatedOnUtc = DateTime.UtcNow
            });

            //update totals
            pollAnswer.NumberOfVotes = (await _pollService.GetPollVotingRecordsByPollAnswerAsync(pollAnswer.Id)).Count;
            await _pollService.UpdatePollAnswerAsync(pollAnswer);
            await _pollService.UpdatePollAsync(poll);
        }

        return Ok(new
        {
            html = await _pollModelFactory.PreparePollDtoAsync(poll, true),
        });
    }

    #endregion

    #region Components

    [HttpGet]
    [Route("GetPollBlock", Name = "GetPollBlock")]
    [ProducesResponseType(typeof(PollDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPollBlock([FromQuery] string systemKeyword)
    {

        if (string.IsNullOrWhiteSpace(systemKeyword))
            return Content("");

        var model = await _pollModelFactory.PreparePollDtoBySystemNameAsync(systemKeyword);
        if (model == null)
            return Content("");

        return Ok(model);
    }

    #endregion
}
