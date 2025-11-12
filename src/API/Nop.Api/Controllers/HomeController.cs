using Microsoft.AspNetCore.Mvc;
using Nop.Api.Infrastructure.Cache;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Core.Domain.News;
using Nop.Api.DTOs.Blogs;
using System.Net;
using Nop.Api.DTOs.Catalog;
using Nop.Api.DTOs.News;
using Nop.Api.DTOs.Polls;

namespace Nop.Api.Controllers;

public partial class HomeController : BasePublicController
{
    #region Fields

    protected readonly CatalogSettings _catalogSettings;
    protected readonly IAclService _aclService;
    protected readonly ICatalogDtoFactory _catalogModelFactory;
    protected readonly ICategoryService _categoryService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IManufacturerService _manufacturerService;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly IPermissionService _permissionService;
    protected readonly IProductDtoFactory _productModelFactory;
    protected readonly IProductService _productService;
    protected readonly IProductTagService _productTagService;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVendorService _vendorService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly MediaSettings _mediaSettings;
    protected readonly VendorSettings _vendorSettings;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly ShoppingCartSettings _shoppingCartSettings;
    private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IOrderReportService _orderReportService;
    private readonly INewsDtoFactory _newsDtoFactory;
    private readonly NewsSettings _newsSettings;
    private readonly IPollDtoFactory _pollDtoFactory;

    #endregion

    #region Ctor

    public HomeController(CatalogSettings catalogSettings,
        IAclService aclService,
        ICatalogDtoFactory catalogModelFactory,
        ICategoryService categoryService,
        ICustomerActivityService customerActivityService,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        IManufacturerService manufacturerService,
        INopUrlHelper nopUrlHelper,
        IPermissionService permissionService,
        IProductDtoFactory productModelFactory,
        IProductService productService,
        IProductTagService productTagService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IUrlRecordService urlRecordService,
        IVendorService vendorService,
        IWebHelper webHelper,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        VendorSettings vendorSettings,
        IShoppingCartService shoppingCartService,
        ShoppingCartSettings shoppingCartSettings,
        IRecentlyViewedProductsService recentlyViewedProductsService,
        IStaticCacheManager staticCacheManager,
        IOrderReportService orderReportService,
        INewsDtoFactory newsDtoFactory,
        NewsSettings newsSettings,
        IPollDtoFactory pollDtoFactory)
    {
        _catalogSettings = catalogSettings;
        _aclService = aclService;
        _catalogModelFactory = catalogModelFactory;
        _categoryService = categoryService;
        _customerActivityService = customerActivityService;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _manufacturerService = manufacturerService;
        _nopUrlHelper = nopUrlHelper;
        _permissionService = permissionService;
        _productModelFactory = productModelFactory;
        _productService = productService;
        _productTagService = productTagService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _urlRecordService = urlRecordService;
        _vendorService = vendorService;
        _webHelper = webHelper;
        _workContext = workContext;
        _mediaSettings = mediaSettings;
        _vendorSettings = vendorSettings;
        _shoppingCartService = shoppingCartService;
        _shoppingCartSettings = shoppingCartSettings;
        _recentlyViewedProductsService = recentlyViewedProductsService;
        _staticCacheManager = staticCacheManager;
        _orderReportService = orderReportService;
        _newsDtoFactory = newsDtoFactory;
        _newsSettings = newsSettings;
        _pollDtoFactory = pollDtoFactory;
    }

    #endregion

    [HttpGet]
    [Route("index")]
    public virtual IActionResult Index()
    {
        return Ok();
    }

    #region Components


    [HttpGet]
    [Route("GetHomepageBestSellers", Name = "GetHomepageBestSellers")]
    [ProducesResponseType(typeof(IList<ProductOverviewDto>), (int)HttpStatusCode.OK)]

    public async Task<IActionResult> GetHomepageBestSellers([FromQuery]int? productThumbPictureSize)
    {
        if (!_catalogSettings.ShowBestsellersOnHomepage || _catalogSettings.NumberOfBestsellersOnHomepage == 0)
            return Content("");

        //load and cache report
        var store = await _storeContext.GetCurrentStoreAsync();
        var report = await _staticCacheManager.GetAsync(
            _staticCacheManager.PrepareKeyForDefaultCache(NopDtoCacheDefaults.HomepageBestsellersIdsKey,
                store),
            async () => await (await _orderReportService.BestSellersReportAsync(
                storeId: store.Id,
                pageSize: _catalogSettings.NumberOfBestsellersOnHomepage)).ToListAsync());

        //load products
        var products = await (await _productService.GetProductsByIdsAsync(report.Select(x => x.ProductId).ToArray()))
            //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();

        if (!products.Any())
            return Content("");

        //prepare model
        var model = (await _productModelFactory.PrepareProductOverviewDtosAsync(products, true, true, productThumbPictureSize)).ToList();
        return Ok(model);
    }



    [HttpGet]
    [Route("GetHomepageCategories", Name = "GetHomepageCategories")]
    [ProducesResponseType(typeof(List<CategoryDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetHomepageCategories()
    {
        var model = await _catalogModelFactory.PrepareHomepageCategoryDtosAsync();
        if (!model.Any())
            return Content("");

        return Ok(model);
    }


    [HttpGet]
    [Route("GetHomepageNews", Name = "GetHomepageNews")]
    [ProducesResponseType(typeof(HomepageNewsItemsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetHomepageNews()
    {
        if (!_newsSettings.Enabled || !_newsSettings.ShowNewsOnMainPage)
            return Content("");

        var model = await _newsDtoFactory.PrepareHomepageNewsItemsModelAsync();
        return Ok(model);
    }

    [HttpGet]
    [Route("GetHomepagePolls", Name = "GetHomepagePolls")]
    [ProducesResponseType(typeof(List<PollDto>), (int)HttpStatusCode.OK)]

    public async Task<IActionResult> GetHomepagePolls()
    {
        var model = await _pollDtoFactory.PrepareHomepagePollDtosAsync();
        if (!model.Any())
            return Content("");

        return Ok(model);
    }


    [HttpGet]
    [Route("GetHomepageProducts", Name = "GetHomepageProducts")]
    [ProducesResponseType(typeof(IList<ProductOverviewDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetHomepageProducts(int? productThumbPictureSize)
    {
        var products = await (await _productService.GetAllProductsDisplayedOnHomepageAsync())
            //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();

        if (!products.Any())
            return Content("");

        var model = (await _productModelFactory.PrepareProductOverviewDtosAsync(products, true, true, productThumbPictureSize)).ToList();

        return Ok(model);
    }

    #endregion
}
