using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Nop.Api.DTOs.Authentication;
using Nop.Api.Framework;
using Nop.Api.Infrastructure;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Authentication;
using Nop.Services.Customers;
using Nop.Services.Logging;
using Nop.Services.Orders;

namespace Nop.Api.Controllers
{
    [AllowAnonymous]
    public class TokenController : BasePublicController
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAuthenticationService _authenticationService;
        private readonly CustomerSettings _customerSettings;
        private readonly ApiSettings _apiSettings;
        private readonly IWorkContext _workContext;

        public TokenController(
            ICustomerService customerService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerActivityService customerActivityService,
            IShoppingCartService shoppingCartService,
            IAuthenticationService authenticationService,
            CustomerSettings customerSettings,
            ApiSettings apiSettings,
            IWorkContext workContext)
        {
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _customerActivityService = customerActivityService;
            _shoppingCartService = shoppingCartService;
            _authenticationService = authenticationService;
            _customerSettings = customerSettings;
            _apiSettings = apiSettings;
            _workContext = workContext;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("/token", Name = "RequestToken")]
        [ProducesResponseType(typeof(TokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Create([FromBody] TokenRequest model)
        {
            var oldCustomer = await _authenticationService.GetAuthenticatedCustomerAsync();

            if (model.Guest)
            {
                var guestCustomer = await CreateGuestCustomerAsync();
                if (guestCustomer == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to create guest customer.");

                await MigrateShoppingCartAsync(oldCustomer, guestCustomer);
                var guestTokenResponse = await GenerateTokenAsync(guestCustomer);
                await SignInCustomerAsync(guestCustomer, model.RememberMe);
                return Ok( guestTokenResponse );
            }

            var newCustomer = await AuthenticateCustomerAsync(model);
            if (newCustomer == null)
                return StatusCode((int)HttpStatusCode.Forbidden, "Wrong username or password");

            await MigrateShoppingCartAsync(oldCustomer, newCustomer);
            var tokenResponse = await GenerateTokenAsync(newCustomer);
            await SignInCustomerAsync(newCustomer, model.RememberMe);
            return Ok( tokenResponse );
        }

        [HttpGet]
        [Authorize(Policy = JwtBearerDefaults.AuthenticationScheme, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("/token/check", Name = "ValidateToken")]
        [ProducesResponseType(typeof(TokenCheckResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ValidateToken()
        {
            var currentCustomer = await _authenticationService.GetAuthenticatedCustomerAsync();
            if (currentCustomer == null)
                return NotFound();

            var dto = new TokenCheckResponse
            {
                CustomerId = currentCustomer.Id,
                CustomerGuid = currentCustomer.CustomerGuid,
                Username = _customerSettings.UsernamesEnabled ? currentCustomer.Username : currentCustomer.Email,
                IsVendor = await _customerService.IsVendorAsync(currentCustomer),
                IsRegistered = await _customerService.IsRegisteredAsync(currentCustomer),
            };
            return Ok(dto);
        }

        #region Private methods

        private async Task<Customer> AuthenticateCustomerAsync(TokenRequest model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return null;

            var loginResult = await _customerRegistrationService.ValidateCustomerAsync(model.Username, model.Password);
            if (loginResult != CustomerLoginResults.Successful)
                return null;

            return _customerSettings.UsernamesEnabled
                ? await _customerService.GetCustomerByUsernameAsync(model.Username)
                : await _customerService.GetCustomerByEmailAsync(model.Username);
        }

        private async Task<Customer> CreateGuestCustomerAsync()
        {
            var guestCustomer = await _customerService.InsertGuestCustomerAsync();

            return guestCustomer;
        }

        private async Task MigrateShoppingCartAsync(Customer oldCustomer, Customer newCustomer)
        {
            if (oldCustomer != null && oldCustomer.Id != newCustomer.Id)
            {
                await _shoppingCartService.MigrateShoppingCartAsync(oldCustomer, newCustomer, true);
            }
        }

        private async Task SignInCustomerAsync(Customer customer, bool rememberMe)
        {
            await _authenticationService.SignInAsync(customer, rememberMe);
            await _customerActivityService.InsertActivityAsync(customer, "Api.TokenRequest", "API token request", customer);
        }

        private async Task<TokenResponse> GenerateTokenAsync(Customer customer)
        {
            var currentTime = DateTimeOffset.UtcNow;
            var expirationTime = currentTime.AddDays(GetTokenExpiryInDays());


            var isGeust = await _customerService.IsGuestAsync(customer);

            if (isGeust)
            {
                customer.Username = string.Empty;
                customer.Email = string.Empty;
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Nbf, currentTime.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationTime.ToUnixTimeSeconds().ToString()),
                new Claim("CustomerId", customer.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerGuid.ToString()),
                new Claim(ClaimTypes.Name, _customerSettings.UsernamesEnabled ? customer.Username ?? customer.Email : customer.Email),
                new Claim(ClaimTypes.Email, customer.Email)
            };

            var signingCredentials = GetSigningCredentials();
            var token = new JwtSecurityToken(new JwtHeader(signingCredentials), new JwtPayload(claims));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse(accessToken, currentTime.UtcDateTime, expirationTime.UtcDateTime)
            {
                CustomerId = customer.Id,
                CustomerGuid = customer.CustomerGuid,
                Username = _customerSettings.UsernamesEnabled ? customer.Username : customer.Email,
                TokenType = "Bearer",
                IsVendor = await _customerService.IsVendorAsync(customer),
                IsRegistered = await _customerService.IsRegisteredAsync(customer),
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var apiConfig = Singleton<AppSettings>.Instance.Get<ApiConfig>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiConfig.SecurityKey));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private int GetTokenExpiryInDays()
        {
            return _apiSettings.TokenExpiryInDays > 0
                ? _apiSettings.TokenExpiryInDays
                : ApiDefaults.Configurations.DefaultAccessTokenExpirationInDays;
        }

        #endregion
    }
}
