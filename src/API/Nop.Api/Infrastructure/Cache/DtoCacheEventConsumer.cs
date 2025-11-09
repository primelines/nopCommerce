using Nop.Core.Caching;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Polls;
using Nop.Core.Domain.Topics;
using Nop.Core.Domain.Vendors;
using Nop.Core.Events;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Plugins;

namespace Nop.Api.Infrastructure.Cache;

/// <summary>
/// Model cache event consumer (used for caching of presentation layer models)
/// </summary>
public partial class DtoCacheEventConsumer :
    //languages
    IConsumer<EntityInsertedEvent<Language>>,
    IConsumer<EntityUpdatedEvent<Language>>,
    IConsumer<EntityDeletedEvent<Language>>,
    //settings
    IConsumer<EntityUpdatedEvent<Setting>>,
    //manufacturers
    IConsumer<EntityInsertedEvent<Manufacturer>>,
    IConsumer<EntityUpdatedEvent<Manufacturer>>,
    IConsumer<EntityDeletedEvent<Manufacturer>>,
    //vendors
    IConsumer<EntityInsertedEvent<Vendor>>,
    IConsumer<EntityUpdatedEvent<Vendor>>,
    IConsumer<EntityDeletedEvent<Vendor>>,
    //categories
    IConsumer<EntityInsertedEvent<Category>>,
    IConsumer<EntityUpdatedEvent<Category>>,
    IConsumer<EntityDeletedEvent<Category>>,
    //product categories
    IConsumer<EntityInsertedEvent<ProductCategory>>,
    IConsumer<EntityDeletedEvent<ProductCategory>>,
    //products
    IConsumer<EntityInsertedEvent<Product>>,
    IConsumer<EntityUpdatedEvent<Product>>,
    IConsumer<EntityDeletedEvent<Product>>,
    //product tags
    IConsumer<EntityInsertedEvent<ProductTag>>,
    IConsumer<EntityUpdatedEvent<ProductTag>>,
    IConsumer<EntityDeletedEvent<ProductTag>>,
    //Product attribute values
    IConsumer<EntityUpdatedEvent<ProductAttributeValue>>,
    //Topics
    IConsumer<EntityInsertedEvent<Topic>>,
    IConsumer<EntityUpdatedEvent<Topic>>,
    IConsumer<EntityDeletedEvent<Topic>>,
    //Orders
    IConsumer<EntityInsertedEvent<Order>>,
    IConsumer<EntityUpdatedEvent<Order>>,
    IConsumer<EntityDeletedEvent<Order>>,
    //Picture
    IConsumer<EntityInsertedEvent<Picture>>,
    IConsumer<EntityUpdatedEvent<Picture>>,
    IConsumer<EntityDeletedEvent<Picture>>,
    //Product picture mapping
    IConsumer<EntityInsertedEvent<ProductPicture>>,
    IConsumer<EntityUpdatedEvent<ProductPicture>>,
    IConsumer<EntityDeletedEvent<ProductPicture>>,
    //Product review
    IConsumer<EntityDeletedEvent<ProductReview>>,
    //polls
    IConsumer<EntityInsertedEvent<Poll>>,
    IConsumer<EntityUpdatedEvent<Poll>>,
    IConsumer<EntityDeletedEvent<Poll>>,
    //blog posts
    IConsumer<EntityInsertedEvent<BlogPost>>,
    IConsumer<EntityUpdatedEvent<BlogPost>>,
    IConsumer<EntityDeletedEvent<BlogPost>>,
    //news items
    IConsumer<EntityInsertedEvent<NewsItem>>,
    IConsumer<EntityUpdatedEvent<NewsItem>>,
    IConsumer<EntityDeletedEvent<NewsItem>>,
    //shopping cart items
    IConsumer<EntityUpdatedEvent<ShoppingCartItem>>
{
    #region Fields

    protected readonly CatalogSettings _catalogSettings;
    protected readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public DtoCacheEventConsumer(CatalogSettings catalogSettings, IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
        _catalogSettings = catalogSettings;
    }

    #endregion

    #region Methods

    #region Languages

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Language> eventMessage)
    {
        //clear all localizable models
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Language> eventMessage)
    {
        //clear all localizable models
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Language> eventMessage)
    {
        //clear all localizable models
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
    }

    #endregion

    #region Setting

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
    {
        //clear models which depend on settings
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey); //depends on CatalogSettings.ManufacturersBlockItemsToDisplay
        await _staticCacheManager.RemoveAsync(NopDtoCacheDefaults.VendorNavigationDtoKey); //depends on VendorSettings.VendorBlockItemsToDisplay
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey); //depends on CatalogSettings.ShowCategoryProductNumber and CatalogSettings.ShowCategoryProductNumberIncludingSubcategories
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey); //depends on CatalogSettings.NumberOfBestsellersOnHomepage
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey); //depends on CatalogSettings.ProductsAlsoPurchasedNumber
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.BlogPrefixCacheKey); //depends on BlogSettings.NumberOfTags
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.NewsPrefixCacheKey); //depends on NewsSettings.MainPageNewsCount
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey); //depends on distinct sitemap settings
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.StoreLogoPathPrefixCacheKey); //depends on StoreInformationSettings.LogoPictureId
    }

    #endregion

    #region Vendors

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
    {
        await _staticCacheManager.RemoveAsync(NopDtoCacheDefaults.VendorNavigationDtoKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage)
    {
        await _staticCacheManager.RemoveAsync(NopDtoCacheDefaults.VendorNavigationDtoKey);
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.VendorPicturePrefixCacheKeyById, eventMessage.Entity.Id));
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Vendor> eventMessage)
    {
        await _staticCacheManager.RemoveAsync(NopDtoCacheDefaults.VendorNavigationDtoKey);
    }

    #endregion

    #region  Manufacturers

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Manufacturer> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Manufacturer> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ManufacturerPicturePrefixCacheKeyById, eventMessage.Entity.Id));
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Manufacturer> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerNavigationPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Categories

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Category> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryHomepagePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Category> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryHomepagePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.CategoryPicturePrefixCacheKeyById, eventMessage.Entity.Id));
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Category> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryHomepagePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Product categories

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<ProductCategory> eventMessage)
    {
        if (_catalogSettings.ShowCategoryProductNumber)
        {
            //depends on CatalogSettings.ShowCategoryProductNumber (when enabled)
            //so there's no need to clear this cache in other cases
            await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
            await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        }
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ProductCategory> eventMessage)
    {
        if (_catalogSettings.ShowCategoryProductNumber)
        {
            //depends on CatalogSettings.ShowCategoryProductNumber (when enabled)
            //so there's no need to clear this cache in other cases
            await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryAllPrefixCacheKey);
            await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryXmlAllPrefixCacheKey);
        }
    }

    #endregion

    #region Products

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Product> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductReviewsPrefixCacheKeyById, eventMessage.Entity.Id));
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Product> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Product tags

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<ProductTag> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<ProductTag> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ProductTag> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Product attributes

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<ProductAttributeValue> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributeImageSquarePicturePrefixCacheKey);
    }

    #endregion

    #region Topics

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Topic> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Topic> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Topic> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Orders

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Order> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Order> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
    }

    #endregion

    #region Pictures

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Picture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Picture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductDetailsPicturesPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductOverviewPicturesPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.VendorPicturePrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Picture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductDetailsPicturesPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductOverviewPicturesPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CategoryPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ManufacturerPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.VendorPicturePrefixCacheKey);
    }

    #endregion

    #region Product picture mappings

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<ProductPicture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<ProductPicture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ProductPicture> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.ProductAttributePicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.OrderPicturePrefixCacheKey);
    }

    #endregion

    #region Polls

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Poll> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.PollsPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Poll> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.PollsPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Poll> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.PollsPrefixCacheKey);
    }

    #endregion

    #region Blog posts

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<BlogPost> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.BlogPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<BlogPost> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.BlogPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<BlogPost> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.BlogPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region News items

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<NewsItem> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.NewsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<NewsItem> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.NewsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<NewsItem> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.NewsPrefixCacheKey);
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.SitemapPrefixCacheKey);
    }

    #endregion

    #region Shopping cart items

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<ShoppingCartItem> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(NopDtoCacheDefaults.CartPicturePrefixCacheKey);
    }

    #endregion

    #region Product reviews

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ProductReview> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(string.Format(NopDtoCacheDefaults.ProductReviewsPrefixCacheKeyById, eventMessage.Entity.ProductId));
    }

    #endregion

    #endregion
}