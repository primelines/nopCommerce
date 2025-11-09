using System.Net;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.ScheduleTasks;

namespace Nop.Api.Controllers;

//do not inherit it from BasePublicController. otherwise a lot of extra action filters will be called
//they can create guest account(s), etc
public partial class ScheduleTaskController : Controller
{
    protected readonly IScheduleTaskService _scheduleTaskService;
    protected readonly IScheduleTaskRunner _taskRunner;

    public ScheduleTaskController(IScheduleTaskService scheduleTaskService,
        IScheduleTaskRunner taskRunner)
    {
        _scheduleTaskService = scheduleTaskService;
        _taskRunner = taskRunner;
    }


    [IgnoreAntiforgeryToken]

    [HttpGet]
    [Route("RunTask", Name = "RunTask")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> RunTask([FromQuery] string taskType)
    {
        var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(taskType);
        if (scheduleTask == null)
            //schedule task cannot be loaded
            return NoContent();

        await _taskRunner.ExecuteAsync(scheduleTask);

        return NoContent();
    }
}
