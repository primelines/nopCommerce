using Nop.Core;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Api.DTOs.Directory;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the country model factory
/// </summary>
public partial class CountryDtoFactory : ICountryDtoFactory
{
    #region Fields

    protected readonly ICountryService _countryService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public CountryDtoFactory(ICountryService countryService,
        ILocalizationService localizationService,
        IStateProvinceService stateProvinceService,
        IWorkContext workContext)
    {
        _countryService = countryService;
        _localizationService = localizationService;
        _stateProvinceService = stateProvinceService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get states and provinces by country identifier
    /// </summary>
    /// <param name="countryId">Country identifier</param>
    /// <param name="addSelectStateItem">Whether to add "Select state" item to list of states</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of identifiers and names of states and provinces
    /// </returns>
    public virtual async Task<IList<StateProvinceDto>> GetStatesByCountryIdAsync(int countryId, bool addSelectStateItem)
    {
        var country = await _countryService.GetCountryByIdAsync(countryId);
        var states =
            (await _stateProvinceService.GetStateProvincesByCountryIdAsync(country?.Id ?? 0,
                (await _workContext.GetWorkingLanguageAsync()).Id)).ToList();
        
        var result = new List<StateProvinceDto>();
        
        foreach (var state in states)
            result.Add(new StateProvinceDto
            {
                id = state.Id,
                name = await _localizationService.GetLocalizedAsync(state, x => x.Name)
            });

        if (country == null)
        {
            //country is not selected ("choose country" item)
            if (addSelectStateItem)
                result.Insert(0, new StateProvinceDto
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.SelectState")
                });
            else
                result.Insert(0, new StateProvinceDto
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
        }
        else
        {
            //some country is selected
            if (!result.Any())
                //country does not have states
                result.Insert(0, new StateProvinceDto
                {
                    id = 0,
                    name = await _localizationService.GetResourceAsync("Address.Other")
                });
            else
            {
                //country has some states
                if (addSelectStateItem)
                    result.Insert(0, new StateProvinceDto
                    {
                        id = 0,
                        name = await _localizationService.GetResourceAsync("Address.SelectState")
                    });
            }
        }

        return result;
    }

    #endregion
}