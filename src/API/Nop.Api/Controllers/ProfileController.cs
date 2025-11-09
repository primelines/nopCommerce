using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Security;
using Nop.Api.Factories;
using Nop.Api.Framework;
using System.Net;
using Nop.Api.DTOs.Profile;

namespace Nop.Api.Controllers;

public partial class ProfileController : BasePublicController
{
    protected readonly CustomerSettings _customerSettings;
    protected readonly ICustomerService _customerService;
    protected readonly IPermissionService _permissionService;
    protected readonly IProfileDtoFactory _profileModelFactory;

    public ProfileController(CustomerSettings customerSettings,
        ICustomerService customerService,
        IPermissionService permissionService,
        IProfileDtoFactory profileModelFactory)
    {
        _customerSettings = customerSettings;
        _customerService = customerService;
        _permissionService = permissionService;
        _profileModelFactory = profileModelFactory;
    }

    [HttpGet]
    [Route("Index", Name = "Profile")]
    [ProducesResponseType(typeof(ProfileIndexDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]

    public virtual async Task<IActionResult> Index([FromQuery] int? id, [FromQuery] int? pageNumber)
    {
        if (!_customerSettings.AllowViewingProfiles)
            return Error(errorMessage: "Disabled from settings");


        var customerId = 0;
        if (id.HasValue)
        {
            customerId = id.Value;
        }

        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null || await _customerService.IsGuestAsync(customer))
        {
            return Error();
        }

        ////display "edit" (manage) link
        //if (await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ManageCustomers))
        //    DisplayEditLink(Url.Action("Edit", "Customer", new { id = customer.Id, area = AreaNames.ADMIN }));

        var model = await _profileModelFactory.PrepareProfileIndexDtoAsync(customer, pageNumber);
        return Ok(model);
    }
}
