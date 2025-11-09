using System.Net;
using Microsoft.AspNetCore.Mvc;
using Nop.Api.DTOs.Directory;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc.Filters;

namespace Nop.Api.Controllers;

public partial class CountryController : BasePublicController
{
    #region Fields

    protected readonly ICountryDtoFactory _countryModelFactory;
        
    #endregion

    #region Ctor

    public CountryController(ICountryDtoFactory countryModelFactory)
    {
        _countryModelFactory = countryModelFactory;
    }

    #endregion

    #region States / provinces

    //available even when navigation is not allowed
    [CheckAccessPublicStore(ignore: true)]
    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("GetStatesByCountryId/{countryId}", Name = "GetStatesByCountryId")]
    [ProducesResponseType(typeof(IList<StateProvinceDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> GetStatesByCountryId([FromRoute] int countryId, [FromQuery] bool addSelectStateItem)
    {
        var model = await _countryModelFactory.GetStatesByCountryIdAsync(countryId, addSelectStateItem);

        return Ok(model);
    }

    #endregion
}
