using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;

namespace Nop.Services.Authentication;

/// <summary>
/// Represents service using cookie middleware for the authentication
/// </summary>
public partial class ApiAuthenticationService : IAuthenticationService
{
    #region Fields

    protected readonly CustomerSettings _customerSettings;
    protected readonly ICustomerService _customerService;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected Customer _cachedCustomer;

    protected CookieAuthenticationService CookieAuthenticationService { get; }

    #endregion

    #region Ctor

    public ApiAuthenticationService(CustomerSettings customerSettings,
        ICustomerService customerService,
        IHttpContextAccessor httpContextAccessor)
    {
        _customerSettings = customerSettings;
        _customerService = customerService;
        _httpContextAccessor = httpContextAccessor;
        CookieAuthenticationService = new CookieAuthenticationService(customerSettings, customerService, httpContextAccessor);
    }

    #endregion

    #region Utilites


    private async Task<Customer> BearerGetAuthenticatedCustomerAsync()
    {
        //whether there is a cached customer
        if (_cachedCustomer != null)
            return _cachedCustomer;

        //try to get authenticated user identity
        var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
            return null;

        Customer customer = null;
        var identifierClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (identifierClaim != null && Guid.TryParse(identifierClaim.Value, out Guid customerGuid))
        {
            customer = await _customerService.GetCustomerByGuidAsync(customerGuid);
        }


        //whether the found customer is available
        if (customer == null || !customer.Active || customer.RequireReLogin || customer.Deleted || !await _customerService.IsRegisteredAsync(customer))
            return null;

        static DateTime trimMilliseconds(DateTime dt) => new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);

        //get the latest password
        var customerPassword = await _customerService.GetCurrentPasswordAsync(customer.Id);
        //require a customer to re-login after password changing
        var isPasswordChange = trimMilliseconds(customerPassword.CreatedOnUtc).CompareTo(trimMilliseconds(authenticateResult.Properties.IssuedUtc?.DateTime ?? DateTime.UtcNow)) > 0;
        if (_customerSettings.RequiredReLoginAfterPasswordChange && isPasswordChange)
            return null;

        //cache authenticated customer
        _cachedCustomer = customer;

        return _cachedCustomer;
    }


    #endregion

    #region Methods

    /// <summary>
    /// Sign in
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SignInAsync(Customer customer, bool isPersistent)
    {
        await CookieAuthenticationService.SignInAsync(customer, isPersistent);
    }

    /// <summary>
    /// Sign out
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SignOutAsync()
    {
        await CookieAuthenticationService.SignOutAsync();
    }

    /// <summary>
    /// Get authenticated customer
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer
    /// </returns>
    /// 
    public virtual async Task<Customer> GetAuthenticatedCustomerAsync()
    {
        var customer = await BearerGetAuthenticatedCustomerAsync();
        if (customer is not null)
            return customer;

        customer = await CookieAuthenticationService.GetAuthenticatedCustomerAsync();
        return customer;
    }


    #endregion
}