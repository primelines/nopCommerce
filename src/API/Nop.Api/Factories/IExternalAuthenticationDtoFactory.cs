using Nop.Api.DTOs.Customer;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the interface of the external authentication model factory
/// </summary>
public partial interface IExternalAuthenticationDtoFactory
{
    /// <summary>
    /// Prepare the external authentication method model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of the external authentication method model
    /// </returns>
    Task<List<ExternalAuthenticationMethodDto>> PrepareExternalMethodsModelAsync();
}