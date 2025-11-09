using Microsoft.AspNetCore.Mvc;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc.Filters;

namespace Nop.Api.Controllers;

[WwwRequirement]
[CheckLanguageSeoCode]
[CheckAccessPublicStore]
[CheckAccessClosedStore]
[CheckDiscountCoupon]
[CheckAffiliate]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract partial class BasePublicController : BaseController
{
    protected virtual IActionResult InvokeHttp404()
    {
        Response.StatusCode = 404;
        return Problem(statusCode:404);
    }
}
