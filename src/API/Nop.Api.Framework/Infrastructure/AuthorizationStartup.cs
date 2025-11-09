using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Api.Authorization.Requirements;
using Nop.Core.Infrastructure;
using Nop.Api.Authorization.Policies;
using Nop.Api.Framework.Authorization.Requirements;
using Nop.Api.Framework.Authorization.Policies;

namespace Nop.Api.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring Authorization middleware on application startup
/// </summary>
public partial class AuthorizationStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(JwtBearerDefaults.AuthenticationScheme,
                              policy =>
                              {
                                  policy.Requirements.Add(new ActiveApiPluginRequirement());
                                  policy.Requirements.Add(new AuthorizationSchemeRequirement());
                                  policy.RequireAuthenticatedUser();
                              });
        });

        services.AddSingleton<IAuthorizationHandler, ActiveApiPluginAuthorizationPolicy>();
        services.AddSingleton<IAuthorizationHandler, ValidSchemeAuthorizationPolicy>();

    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //Add the Authorization middleware
        application.UseAuthorization();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 600; // Authorization should be loaded before Endpoint and after authentication
}