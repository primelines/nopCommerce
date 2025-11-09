using System.Reflection;
using System.Text.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nop.Api.Framework.Infrastructure.Extensions;
using Nop.Api.Infrastructure.Swagger;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;

namespace Nop.Api;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
        if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
        {
            var path = string.Format(NopConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
            builder.Configuration.AddJsonFile(path, true, true);
        }
        builder.Configuration.AddEnvironmentVariables();

        // Load application settings
        builder.Services.ConfigureApplicationSettings(builder);

        var appSettings = Singleton<AppSettings>.Instance;
        var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

        if (useAutofac)
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        else
            builder.Host.UseDefaultServiceProvider(options =>
            {
                // Avoid scope validation for root container resolution
                options.ValidateScopes = false;
                options.ValidateOnBuild = true;
            });

        // Register controllers and enforce enums as strings
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Add Swagger services
        builder.Services.AddSwaggerGen(options =>
        {
            // Basic info
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Tikto API v1",
                Description = "Public API for Tikto React Native App",
                Contact = new OpenApiContact
                {
                    Name = "Tikto Dev Team",
                    Email = "support@tikto.dev"
                }
            });

            // JWT auth config
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Use controller name as tag
            options.TagActionsBy(api =>
            {
                var controllerName = api.ActionDescriptor.RouteValues["controller"];
                return controllerName != null ? new[] { controllerName } : new[] { "Default" };
            });

            // Custom operation ID per action
            options.CustomOperationIds(apiDesc =>
            {
                return $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}";
            });

            // Schema and nullability enhancements
            options.UseAllOfToExtendReferenceSchemas();
            options.SupportNonNullableReferenceTypes();

            // Include XML comments (if available)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            options.OperationFilter<OperationIdRewriteFilter>();
            options.DocumentFilter<FlattenAllOfDocumentFilter>();

        });

        builder.Services.AddSwaggerGenNewtonsoftSupport();

        // Add services and configure DI
        builder.Services.ConfigureApplicationServices(builder);

        var app = builder.Build();

        // Always redirect root to Swagger UI
        app.MapGet("/", () => Results.Redirect("/api/swagger/"));

        // Swagger middleware config
        app.UseSwagger(options => options.RouteTemplate = "api/swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "api/swagger";
            c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Nop Public Api v4.74");
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });

        // Configure HTTP pipeline
        app.ConfigureRequestPipeline();
        await app.StartEngineAsync();

        await app.RunAsync();
    }
}