using System.Net;
using System.Threading.RateLimiting;
using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Domain.Common;
using Nop.Core.Http;
using Nop.Core.Infrastructure;
using Nop.Core.Security;
using Nop.Data;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Common;
using Nop.Api.Framework.Mvc.ModelBinding;
using Nop.Api.Framework.Mvc.ModelBinding.Binders;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.Framework.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


namespace Nop.Api.Framework.Infrastructure.Extensions;

/// <summary>
/// Represents extensions of IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure base application settings
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationSettings(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //let the operating system decide what TLS protocol version to use
        //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
        ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

        //create default file provider
        CommonHelper.DefaultFileProvider = new NopFileProvider(builder.Environment);

        //register type finder
        var typeFinder = new WebAppTypeFinder();
        Singleton<ITypeFinder>.Instance = typeFinder;
        services.AddSingleton<ITypeFinder>(typeFinder);

        //add configuration parameters
        var configurations = typeFinder
            .FindClassesOfType<IConfig>()
            .Select(configType => (IConfig)Activator.CreateInstance(configType))
            .ToList();

        foreach (var config in configurations)
            builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

        var appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
        services.AddSingleton(appSettings);
    }

    /// <summary>
    /// Add services to the application and configure service provider
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //add accessor to HttpContext
        services.AddHttpContextAccessor();

        //initialize plugins
        var mvcCoreBuilder = services.AddMvcCore();
        var pluginConfig = new PluginConfig();
        builder.Configuration.GetSection(nameof(PluginConfig)).Bind(pluginConfig, options => options.BindNonPublicProperties = true);
        mvcCoreBuilder.PartManager.InitializePlugins(pluginConfig);

        //create engine and configure service provider
        var engine = EngineContext.Create();

        builder.Services.AddRateLimiter(options =>
        {
            var settings = Singleton<AppSettings>.Instance.Get<CommonConfig>();

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = settings.PermitLimit,
                        QueueLimit = settings.QueueCount,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            options.RejectionStatusCode = settings.RejectionStatusCode;
        });

        engine.ConfigureServices(services, builder.Configuration);
    }

    /// <summary>
    /// Register HttpContextAccessor
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddHttpContextAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    ///// <summary>
    ///// Adds services required for anti-forgery support
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddAntiForgery(this IServiceCollection services)
    //{
    //    //override cookie name
    //    services.AddAntiforgery(options =>
    //    {
    //        options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.AntiforgeryCookie}";
    //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //    });
    //}

    /// <summary>
    /// Adds services required for application session state
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddHttpSession(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.SessionCookie}";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });
    }

    ///// <summary>
    ///// Adds services required for themes support
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddThemes(this IServiceCollection services)
    //{
    //    if (!DataSettingsManager.IsDatabaseInstalled())
    //        return;

    //    //themes support
    //    services.Configure<RazorViewEngineOptions>(options =>
    //    {
    //        options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
    //    });
    //}

    /// <summary>
    /// Adds services required for distributed cache
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddDistributedCache(this IServiceCollection services)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

        if (!distributedCacheConfig.Enabled)
            return;

        switch (distributedCacheConfig.DistributedCacheType)
        {
            case DistributedCacheType.Memory:
                services.AddDistributedMemoryCache();
                break;

            case DistributedCacheType.SqlServer:
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = distributedCacheConfig.ConnectionString;
                    options.SchemaName = distributedCacheConfig.SchemaName;
                    options.TableName = distributedCacheConfig.TableName;
                });
                break;

            case DistributedCacheType.Redis:
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = distributedCacheConfig.ConnectionString;
                    options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                });
                break;

            case DistributedCacheType.RedisSynchronizedMemory:
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = distributedCacheConfig.ConnectionString;
                    options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                });
                break;
        }
    }

    /// <summary>
    /// Adds data protection services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddNopDataProtection(this IServiceCollection services)
    {
        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(NopDataProtectionDefaults.DataProtectionKeysPath);
        var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

        //configure the data protection system to persist keys to the specified directory
        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
    }

    /// <summary>
    /// Adds authentication service
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddApiAuthentication(this IServiceCollection services)
    {
        ////set default authentication schemes
        //var authenticationBuilder = services.AddAuthentication(options =>
        //{
        //    options.DefaultChallengeScheme = NopAuthenticationDefaults.AuthenticationScheme;
        //    options.DefaultScheme = NopAuthenticationDefaults.AuthenticationScheme;
        //    options.DefaultSignInScheme = NopAuthenticationDefaults.ExternalAuthenticationScheme;
        //});

        var apiConfig = Singleton<AppSettings>.Instance.Get<ApiConfig>();

        //set default authentication schemes
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
        {
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiConfig.SecurityKey)),
                ValidateIssuer = false, // ValidIssuer = "The name of the issuer",
                ValidateAudience = false, // ValidAudience = "The name of the audience",
                ValidateLifetime = true, // validate the expiration and not before values in the token
                ClockSkew = TimeSpan.FromMinutes(apiConfig.AllowedClockSkewInMinutes)
            };
        });
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


        //add main cookie authentication
        authenticationBuilder.AddCookie(NopAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.AuthenticationCookie}";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.LoginPath = NopAuthenticationDefaults.LoginPath;
            options.AccessDeniedPath = NopAuthenticationDefaults.AccessDeniedPath;
        });

        //add external authentication
        authenticationBuilder.AddCookie(NopAuthenticationDefaults.ExternalAuthenticationScheme, options =>
        {
            options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.ExternalAuthenticationCookie}";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.LoginPath = NopAuthenticationDefaults.LoginPath;
            options.AccessDeniedPath = NopAuthenticationDefaults.AccessDeniedPath;
        });

        //register and configure external authentication plugins now
        var typeFinder = Singleton<ITypeFinder>.Instance;
        var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
        var externalAuthInstances = externalAuthConfigurations
            .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

        foreach (var instance in externalAuthInstances)
            instance.Configure(authenticationBuilder);
    }

    /// <summary>
    /// Add and configure MVC for the application
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>A builder for configuring MVC services</returns>
    public static IMvcBuilder AddNopMvc(this IServiceCollection services)
    {
        //add basic MVC feature
        var mvcBuilder = services.AddControllers().AddNewtonsoftJson();
        

        //mvcBuilder.AddRazorRuntimeCompilation();

        var appSettings = Singleton<AppSettings>.Instance;
        if (appSettings.Get<CommonConfig>().UseSessionStateTempDataProvider)
        {
            //use session-based temp data provider
            mvcBuilder.AddSessionStateTempDataProvider();
        }
        else
        {
            //use cookie-based temp data provider
            mvcBuilder.AddCookieTempDataProvider(options =>
            {
                options.Cookie.Name = $"{NopCookieDefaults.Prefix}{NopCookieDefaults.TempDataCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        //services.AddRazorPages();
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();

        //MVC now serializes JSON with camel case names by default, use this code to avoid it
        mvcBuilder.AddNewtonsoftJson(options =>
        {
            // Ensures property names in JSON use the ones defined by [JsonProperty]
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = null // No automatic conversion
            };
        }
        );

        //set some options
        mvcBuilder.AddMvcOptions(options =>
        {
            options.ModelBinderProviders.Insert(1, new NopDtoBinderProvider());
            //add custom display metadata provider 
            options.ModelMetadataDetailsProviders.Add(new NopMetadataProvider());

            //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
            //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => NopValidationDefaults.NotNullValidationLocaleName);
        });

        ////add fluent validation
        //services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //register all available validators from Nop assemblies
        var assemblies = mvcBuilder.PartManager.ApplicationParts
            .OfType<AssemblyPart>()
            .Where(part => part.Name.StartsWith("Nop", StringComparison.InvariantCultureIgnoreCase))
            .Select(part => part.Assembly);
        services.AddValidatorsFromAssemblies(assemblies);

        //register controllers as services, it'll allow to override them
        mvcBuilder.AddControllersAsServices();

        return mvcBuilder;
    }

    /// <summary>
    /// Register custom RedirectResultExecutor
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddNopRedirectResultExecutor(this IServiceCollection services)
    {
        //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
        services.AddScoped<IActionResultExecutor<RedirectResult>, NopRedirectResultExecutor>();
    }

    ///// <summary>
    ///// Add and configure WebMarkupMin service
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNopWebMarkupMin(this IServiceCollection services)
    //{
    //    //check whether database is installed
    //    if (!DataSettingsManager.IsDatabaseInstalled())
    //        return;

    //    services
    //        .AddWebMarkupMin(options =>
    //        {
    //            options.AllowMinificationInDevelopmentEnvironment = true;
    //            options.AllowCompressionInDevelopmentEnvironment = true;
    //            options.DisableMinification = !EngineContext.Current.Resolve<CommonSettings>().EnableHtmlMinification;
    //            options.DisableCompression = true;
    //            options.DisablePoweredByHttpHeaders = true;
    //        })
    //        .AddHtmlMinification(options =>
    //        {
    //            options.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes;

    //            options.CssMinifierFactory = new NUglifyCssMinifierFactory();
    //            options.JsMinifierFactory = new NUglifyJsMinifierFactory();
    //        })
    //        .AddXmlMinification(options =>
    //        {
    //            var settings = options.MinificationSettings;
    //            settings.RenderEmptyTagsWithSpace = true;
    //            settings.CollapseTagsWithoutContent = true;
    //        });
    //}

    ///// <summary>
    ///// Adds WebOptimizer to the specified <see cref="IServiceCollection"/> and enables CSS and JavaScript minification.
    ///// </summary>
    ///// <param name="services">Collection of service descriptors</param>
    //public static void AddNopWebOptimizer(this IServiceCollection services)
    //{
    //    var appSettings = Singleton<AppSettings>.Instance;
    //    var woConfig = appSettings.Get<WebOptimizerConfig>();

    //    if (!woConfig.EnableCssBundling && !woConfig.EnableJavaScriptBundling)
    //    {
    //        services.AddScoped<INopAssetHelper, NopDefaultAssetHelper>();
    //        return;
    //    }

    //    //add minification & bundling
    //    var cssSettings = new CssBundlingSettings
    //    {
    //        FingerprintUrls = false,
    //        Minify = woConfig.EnableCssBundling
    //    };

    //    var codeSettings = new CodeBundlingSettings
    //    {
    //        Minify = woConfig.EnableJavaScriptBundling,
    //        AdjustRelativePaths = false //disable this feature because it breaks function names that have "Url(" at the end
    //    };

    //    services.AddWebOptimizer(null, cssSettings, codeSettings);
    //    services.AddScoped<INopAssetHelper, NopAssetHelper>();
    //}

    /// <summary>
    /// Add and configure default HTTP clients
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddNopHttpClients(this IServiceCollection services)
    {
        //default client
        services.AddHttpClient(NopHttpDefaults.DefaultHttpClient).WithProxy();

        //client to request current store
        services.AddHttpClient<StoreHttpClient>();

    }
}