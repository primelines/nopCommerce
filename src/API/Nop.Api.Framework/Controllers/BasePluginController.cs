using Microsoft.AspNetCore.Mvc;
using Nop.Api.Framework.Mvc.Filters;

namespace Nop.Api.Framework.Controllers;

/// <summary>
/// Base controller for plugins
/// </summary>
[NotNullValidationMessage]
[Route("api/plugin/[controller]")]

public abstract partial class BasePluginController : BaseController
{
}
