using Nop.Core.Infrastructure;
using Nop.Api.Framework.Factories;

namespace Nop.Api.Infrastructure;

/// <summary>
/// Represents the registering services on application startup
/// </summary>
public partial class NopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        //common factories
        services.AddScoped<IAclSupportedDtoFactory, AclSupportedDtoFactory>();
        services.AddScoped<IDiscountSupportedDtoFactory, DiscountSupportedDtoFactory>();
        services.AddScoped<ILocalizedDtoFactory, LocalizedDtoFactory>();
        services.AddScoped<IStoreMappingSupportedDtoFactory, StoreMappingSupportedDtoFactory>();

        //factories
        services.AddScoped<Factories.IAddressDtoFactory, Factories.AddressDtoFactory>();
        services.AddScoped<Factories.IBlogDtoFactory, Factories.BlogDtoFactory>();
        services.AddScoped<Factories.ICatalogDtoFactory, Factories.CatalogDtoFactory>();
        services.AddScoped<Factories.ICheckoutDtoFactory, Factories.CheckoutDtoFactory>();
        services.AddScoped<Factories.ICommonDtoFactory, Factories.CommonDtoFactory>();
        services.AddScoped<Factories.ICountryDtoFactory, Factories.CountryDtoFactory>();
        services.AddScoped<Factories.ICustomerDtoFactory, Factories.CustomerDtoFactory>();
        services.AddScoped<Factories.IForumDtoFactory, Factories.ForumDtoFactory>();
        services.AddScoped<Factories.IExternalAuthenticationDtoFactory, Factories.ExternalAuthenticationDtoFactory>();
        services.AddScoped<Factories.IJsonLdDtoFactory, Factories.JsonLdDtoFactory>();
        services.AddScoped<Factories.INewsDtoFactory, Factories.NewsDtoFactory>();
        services.AddScoped<Factories.INewsletterDtoFactory, Factories.NewsletterDtoFactory>();
        services.AddScoped<Factories.IOrderDtoFactory, Factories.OrderDtoFactory>();
        services.AddScoped<Factories.IPollDtoFactory, Factories.PollDtoFactory>();
        services.AddScoped<Factories.IPrivateMessagesDtoFactory, Factories.PrivateMessagesDtoFactory>();
        services.AddScoped<Factories.IProductDtoFactory, Factories.ProductDtoFactory>();
        services.AddScoped<Factories.IProfileDtoFactory, Factories.ProfileDtoFactory>();
        services.AddScoped<Factories.IReturnRequestDtoFactory, Factories.ReturnRequestDtoFactory>();
        services.AddScoped<Factories.IShoppingCartDtoFactory, Factories.ShoppingCartDtoFactory>();
        services.AddScoped<Factories.ISitemapDtoFactory, Factories.SitemapDtoFactory>();
        services.AddScoped<Factories.ITopicDtoFactory, Factories.TopicDtoFactory>();
        services.AddScoped<Factories.IVendorDtoFactory, Factories.VendorDtoFactory>();


    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 2002;
}