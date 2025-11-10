using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Core.Rss;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Api.Factories;
using Nop.Api.Framework.Mvc;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.Framework.Mvc.Routing;
using Nop.Api.DTOs.Catalog;
using System.Net;
using Nop.Api.DTOs.Responses;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;

namespace Nop.Api.Controllers;

public partial class CatalogController : BasePublicController
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

    #endregion

    #region Ctor

    public CatalogController(CatalogSettings catalogSettings,
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
        IRecentlyViewedProductsService recentlyViewedProductsService)
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
    }

    #endregion

    #region Categories

    [HttpPost]
    [Route("GetCategory/{categoryId}", Name = "GetCategory")]
    [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Category([FromRoute] int categoryId, CatalogProductsCommand command)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);

        if (!await CheckCategoryAvailabilityAsync(category))
            return InvokeHttp404();

        var store = await _storeContext.GetCurrentStoreAsync();

        //'Continue shopping' URL
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
            NopCustomerDefaults.LastContinueShoppingPageAttribute,
            _webHelper.GetThisPageUrl(false),
            store.Id);

        ////display "edit" (manage) link
        //if (await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ManageCategories))
        //    DisplayEditLink(Url.Action("Edit", "Category", new { id = category.Id, area = AreaNames.ADMIN }));

        //activity log
        await _customerActivityService.InsertActivityAsync("PublicStore.ViewCategory",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ViewCategory"), category.Name), category);

        //model
        var model = await _catalogModelFactory.PrepareCategoryDtoAsync(category, command);

        //template
        var templateViewPath = await _catalogModelFactory.PrepareCategoryTemplateViewPathAsync(category.CategoryTemplateId);

        var response = new CategoryResponse
        {
            Category = model,
            TemplateViewPath = templateViewPath
        };

        return Ok(response);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpPost]
    [Route("GetCategoryProducts/{categoryId}", Name = "GetCategoryProducts")]
    [ProducesResponseType(typeof(GetCategoryProductsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetCategoryProducts([FromRoute] int categoryId, CatalogProductsCommand command)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);

        if (!await CheckCategoryAvailabilityAsync(category))
            return NotFound();

        var model = await _catalogModelFactory.PrepareCategoryProductsModelAsync(category, command);

        return Ok( model);
    }


    //TODO: Difirent Parameters
    //([FromQuery]  loadImage)
    [HttpGet]
    [Route("GetCatalogRoot", Name = "GetCatalogRoot")]
    [ProducesResponseType(typeof(IList<CategorySimpleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> GetCatalogRoot()
    {
        var model = await _catalogModelFactory.PrepareRootCategoriesAsync();

        return Ok(model);
    }


    [HttpGet]
    [Route("GetCatalogSubCategories/{id}", Name = "GetCatalogSubCategories")]
    [ProducesResponseType(typeof(IList<CategorySimpleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> GetCatalogSubCategories([FromRoute] int id)
    {
        var model = await _catalogModelFactory.PrepareSubCategoriesAsync(id);

        return Ok(model);
    }

    #endregion

    #region Manufacturers

    [HttpPost]
    [Route("GetManufacturer/{manufacturerId}", Name = "GetManufacturer")]
    [ProducesResponseType(typeof(ManufacturerResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Manufacturer([FromRoute] int manufacturerId, CatalogProductsCommand command)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);

        if (!await CheckManufacturerAvailabilityAsync(manufacturer))
            return InvokeHttp404();

        var store = await _storeContext.GetCurrentStoreAsync();

        //'Continue shopping' URL
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
            NopCustomerDefaults.LastContinueShoppingPageAttribute,
            _webHelper.GetThisPageUrl(false),
            store.Id);

        ////display "edit" (manage) link
        //if (await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ManageManufacturers))
        //    DisplayEditLink(Url.Action("Edit", "Manufacturer", new { id = manufacturer.Id, area = AreaNames.ADMIN }));

        //activity log
        await _customerActivityService.InsertActivityAsync("PublicStore.ViewManufacturer",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ViewManufacturer"), manufacturer.Name), manufacturer);

        //model
        var model = await _catalogModelFactory.PrepareManufacturerDtoAsync(manufacturer, command);

        ////template
        //var templateViewPath = await _catalogModelFactory.PrepareManufacturerTemplateViewPathAsync(manufacturer.ManufacturerTemplateId);

        return Ok(model);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpPost]
    [Route("GetManufacturerProducts/{manufacturerId}", Name = "GetManufacturerProducts")]
    [ProducesResponseType(typeof(GetManufacturerProductsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetManufacturerProducts([FromRoute] int manufacturerId, CatalogProductsCommand command)
    {
        var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);

        if (!await CheckManufacturerAvailabilityAsync(manufacturer))
            return NotFound();

        var model = await _catalogModelFactory.PrepareManufacturerProductsModelAsync(manufacturer, command);

        return Ok(model);
    }

    [HttpGet]
    [Route("ManufacturerAll", Name = "ManufacturerAll")]
    [ProducesResponseType(typeof(IList<ManufacturerDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ManufacturerAll()
    {
        var model = await _catalogModelFactory.PrepareManufacturerAllModelsAsync();

        return Ok(model);
    }

    #endregion

    #region Vendors

    [HttpPost]
    [Route("GetVendor/{vendorId}", Name = "GetVendor")]
    [ProducesResponseType(typeof(VendorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Vendor([FromRoute] int vendorId, CatalogProductsCommand command)
    {
        var vendor = await _vendorService.GetVendorByIdAsync(vendorId);

        if (!await CheckVendorAvailabilityAsync(vendor))
            return InvokeHttp404();

        var store = await _storeContext.GetCurrentStoreAsync();

        //'Continue shopping' URL
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
            NopCustomerDefaults.LastContinueShoppingPageAttribute,
            _webHelper.GetThisPageUrl(false),
            store.Id);

        ////display "edit" (manage) link
        //if (await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.ManageVendors))
        //    DisplayEditLink(Url.Action("Edit", "Vendor", new { id = vendor.Id, area = AreaNames.ADMIN }));

        //model
        var model = await _catalogModelFactory.PrepareVendorDtoAsync(vendor, command);

        return Ok(model);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpPost]
    [Route("GetVendorProducts/{vendorId}", Name = "GetVendorProducts")]
    [ProducesResponseType(typeof(GetVendorProductsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetVendorProducts([FromRoute] int vendorId, CatalogProductsCommand command)
    {
        var vendor = await _vendorService.GetVendorByIdAsync(vendorId);

        if (!await CheckVendorAvailabilityAsync(vendor))
            return NotFound();

        var model = await _catalogModelFactory.PrepareVendorProductsModelAsync(vendor, command);

        return Ok(model);
    }



    [HttpGet]
    [Route("VendorAll", Name = "VendorAll")]
    [ProducesResponseType(typeof(IList<VendorDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> VendorAll()
    {
        //we don't allow viewing of vendors if "vendors" block is hidden
        if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
            return Error(errorMessage: "Disabled from settings");

        var model = await _catalogModelFactory.PrepareVendorAllModelsAsync();
        return Ok(model);
    }

    #endregion

    #region Product tags

    [HttpPost]
    [Route("GetProductsByTag/{productTagId}", Name = "GetProductsByTag")]
    [ProducesResponseType(typeof(ProductsByTagDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ProductsByTag([FromRoute] int productTagId, CatalogProductsCommand command)
    {
        var productTag = await _productTagService.GetProductTagByIdAsync(productTagId);
        if (productTag == null)
            return InvokeHttp404();

        var model = await _catalogModelFactory.PrepareProductsByTagDtoAsync(productTag, command);

        return Ok(model);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]

    [HttpPost]
    [Route("GetTagProducts/{tagId}", Name = "GetTagProducts")]
    [ProducesResponseType(typeof(GetTagProductsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetTagProducts([FromRoute] int tagId, CatalogProductsCommand command)
    {
        var productTag = await _productTagService.GetProductTagByIdAsync(tagId);
        if (productTag == null)
            return NotFound();

        var model = await _catalogModelFactory.PrepareTagProductsModelAsync(productTag, command);

        return Ok(model);
    }

    [HttpGet]
    [Route("ProductTagsAll", Name = "ProductTagsAll")]
    [ProducesResponseType(typeof(PopularProductTagsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ProductTagsAll()
    {
        var model = await _catalogModelFactory.PreparePopularProductTagsDtoAsync();

        return Ok(model);
    }

    #endregion

    #region Searching

    [HttpPost]
    [Route("Search", Name = "CatalogSearch")]
    [ProducesResponseType(typeof(CatalogSearchDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Search(CatalogSearchDto model, CatalogProductsCommand command)
    {
        var store = await _storeContext.GetCurrentStoreAsync();

        //'Continue shopping' URL
        await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
            NopCustomerDefaults.LastContinueShoppingPageAttribute,
            _webHelper.GetThisPageUrl(true),
            store.Id);

        if (model == null)
            model = new CatalogSearchDto();

        model = await _catalogModelFactory.PrepareSearchDtoAsync(model, command);

        return Ok(model);
    }

    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("SearchTermAutoComplete", Name = "SearchTermAutoComplete")]
    [ProducesResponseType(typeof(IList<SearchTermAutoCompleteResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> SearchTermAutoComplete([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Content("");

        term = term.Trim();

        if (string.IsNullOrWhiteSpace(term) || term.Length < _catalogSettings.ProductSearchTermMinimumLength)
            return Content("");

        //products
        var productNumber = _catalogSettings.ProductSearchAutoCompleteNumberOfProducts > 0 ?
            _catalogSettings.ProductSearchAutoCompleteNumberOfProducts : 10;
        var store = await _storeContext.GetCurrentStoreAsync();
        var products = await _productService.SearchProductsAsync(0,
            storeId: store.Id,
            keywords: term,
            languageId: (await _workContext.GetWorkingLanguageAsync()).Id,
            visibleIndividuallyOnly: true,
            pageSize: productNumber);

        var showLinkToResultSearch = _catalogSettings.ShowLinkToAllResultInSearchAutoComplete && (products.TotalCount > productNumber);

        var models = (await _productModelFactory.PrepareProductOverviewDtosAsync(products, false, _catalogSettings.ShowProductImagesInSearchAutoComplete, _mediaSettings.AutoCompleteSearchThumbPictureSize)).ToList();
        var result = (from p in models
                select new
                {
                    label = p.Name,
                    producturl = Url.RouteUrl<Product>(new { SeName = p.SeName }),
                    productpictureurl = p.Pictures.FirstOrDefault()?.ImageUrl,
                    showlinktoresultsearch = showLinkToResultSearch
                })
            .ToList();
        return Ok(result);
    }

    //ignore SEO friendly URLs checks
    [CheckLanguageSeoCode(ignore: true)]
    [HttpPost]
    [Route("SearchProducts", Name = "SearchProducts")]
    [ProducesResponseType(typeof(SearchProductsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> SearchProducts(CatalogSearchDto searchModel, CatalogProductsCommand command)
    {
        if (searchModel == null)
            searchModel = new CatalogSearchDto();

        var model = await _catalogModelFactory.PrepareSearchProductsModelAsync(searchModel, command);

        return Ok(model);
    }

    #endregion

    #region Utilities

    protected virtual async Task<bool> CheckCategoryAvailabilityAsync(Category category)
    {
        if (category is null)
            return false;

        var isAvailable = true;

        if (category.Deleted)
            isAvailable = false;

        var notAvailable =
            //published?
            !category.Published ||
            //ACL (access control list) 
            !await _aclService.AuthorizeAsync(category) ||
            //Store mapping
            !await _storeMappingService.AuthorizeAsync(category);
        //Check whether the current user has a "Manage categories" permission (usually a store owner)
        //We should allows him (her) to use "Preview" functionality
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.Catalog.CATEGORIES_CREATE_EDIT_DELETE);
        if (notAvailable && !hasAdminAccess)
            isAvailable = false;

        return isAvailable;
    }

    protected virtual async Task<bool> CheckManufacturerAvailabilityAsync(Manufacturer manufacturer)
    {
        if (manufacturer == null)
            return false;

        var isAvailable = true;

        if (manufacturer.Deleted)
            isAvailable = false;

        var notAvailable =
            //published?
            !manufacturer.Published ||
            //ACL (access control list) 
            !await _aclService.AuthorizeAsync(manufacturer) ||
            //Store mapping
            !await _storeMappingService.AuthorizeAsync(manufacturer);
        //Check whether the current user has a "Manage categories" permission (usually a store owner)
        //We should allows him (her) to use "Preview" functionality
        var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL) && await _permissionService.AuthorizeAsync(StandardPermission.Catalog.MANUFACTURER_CREATE_EDIT_DELETE);
        if (notAvailable && !hasAdminAccess)
            isAvailable = false;

        return isAvailable;
    }

    protected virtual Task<bool> CheckVendorAvailabilityAsync(Vendor vendor)
    {
        var isAvailable = true;

        if (vendor == null || vendor.Deleted || !vendor.Active)
            isAvailable = false;

        return Task.FromResult(isAvailable);
    }

    #endregion

    #region Components
    [HttpGet]
    [Route("GetCategoryNavigation", Name = "GetCategoryNavigation")]
    [ProducesResponseType(typeof(CategoryNavigationDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategoryNavigation(int currentCategoryId, int currentProductId)
    {
        var model = await _catalogModelFactory.PrepareCategoryNavigationDtoAsync(currentCategoryId, currentProductId);
        return Ok(model);
    }



    [HttpGet]
    [Route("GetCrossSellProducts", Name = "GetCrossSellProducts")]
    [ProducesResponseType(typeof(List<ProductOverviewDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCrossSellProducts(int? productThumbPictureSize)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

        var products = await (await _productService.GetCrossSellProductsByShoppingCartAsync(cart, _shoppingCartSettings.CrossSellsNumber))
        //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p))
            //visible individually
            .Where(p => p.VisibleIndividually).ToListAsync();

        if (!products.Any())
            return Content("");

        //Cross-sell products are displayed on the shopping cart page.
        //We know that the entire shopping cart page is not refresh
        //even if "ShoppingCartSettings.DisplayCartAfterAddingProduct" setting  is enabled.
        //That's why we force page refresh (redirect) in this case
        var model = (await _productModelFactory.PrepareProductOverviewDtosAsync(products,
                productThumbPictureSize: productThumbPictureSize, forceRedirectionAfterAddingToCart: true))
            .ToList();

        return Ok(model);
    }

    [HttpGet]
    [Route("GetSearchBox", Name = "GetSearchBox")]
    [ProducesResponseType(typeof(SearchBoxDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSearchBox()
    {
        var model = await _catalogModelFactory.PrepareSearchBoxDtoAsync();
        return Ok(model);
    }

    [HttpGet]
    [Route("GetRecentlyViewedProducts", Name = "GetRecentlyViewedProducts")]
    [ProducesResponseType(typeof(List<ProductOverviewDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRecentlyViewedProducts(int? productThumbPictureSize, bool? preparePriceModel)
    {
        if (!_catalogSettings.RecentlyViewedProductsEnabled)
            return Content("");

        var preparePictureDto = productThumbPictureSize.HasValue;
        var products = await (await _recentlyViewedProductsService.GetRecentlyViewedProductsAsync(_catalogSettings.RecentlyViewedProductsNumber))
            //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();

        if (!products.Any())
            return Content("");

        //prepare model
        var model = new List<ProductOverviewDto>();
        model.AddRange(await _productModelFactory.PrepareProductOverviewDtosAsync(products,
            preparePriceModel.GetValueOrDefault(),
            preparePictureDto,
            productThumbPictureSize));

        return Ok(model);
    }

    [HttpGet]
    [Route("GetRelatedProducts", Name = "GetRelatedProducts")]
    [ProducesResponseType(typeof(IList<ProductOverviewDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRelatedProducts(int productId, int? productThumbPictureSize)
    {
        //load and cache report
        var productIds = (await _productService.GetRelatedProductsByProductId1Async(productId)).Select(x => x.ProductId2).ToArray();

        //load products
        var products = await (await _productService.GetProductsByIdsAsync(productIds))
            //ACL and store mapping
            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
            //availability dates
            .Where(p => _productService.ProductIsAvailable(p))
            //visible individually
            .Where(p => p.VisibleIndividually).ToListAsync();

        if (!products.Any())
            return Content(string.Empty);

        var model = (await _productModelFactory.PrepareProductOverviewDtosAsync(products, true, true, productThumbPictureSize)).ToList();
        return Ok(model);
    }


    [HttpGet]
    [Route("GetPopularProductTags", Name = "GetPopularProductTags")]
    [ProducesResponseType(typeof(PopularProductTagsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPopularProductTags()
    {
        var model = await _catalogModelFactory.PreparePopularProductTagsDtoAsync(_catalogSettings.NumberOfProductTags);

        if (!model.Tags.Any())
            return Content("");

        return Ok(model);
    }

    [HttpGet]
    [Route("GetManufacturerNavigation", Name = "GetManufacturerNavigation")]
    [ProducesResponseType(typeof(ManufacturerNavigationDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetManufacturerNavigation(int currentManufacturerId)
    {
        if (_catalogSettings.ManufacturersBlockItemsToDisplay == 0)
            return Content("");

        var model = await _catalogModelFactory.PrepareManufacturerNavigationDtoAsync(currentManufacturerId);
        if (!model.Manufacturers.Any())
            return Content("");

        return Ok(model);
    }


    //[HttpGet]
    //[Route("GetTopMenuView", Name = "GetTopMenuView")]
    //[ProducesResponseType(typeof(TopMenuDto), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetTopMenuView(int? productThumbPictureSize)
    //{
    //    var model = await _catalogModelFactory.PrepareTopMenuDtoAsync();
    //    return Ok(model);
    //}


    [HttpGet]
    [Route("GetVendorNavigation", Name = "GetVendorNavigation")]
    [ProducesResponseType(typeof(VendorNavigationDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVendorNavigation()
    {
        if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
            return Content("");

        var model = await _catalogModelFactory.PrepareVendorNavigationDtoAsync();
        if (!model.Vendors.Any())
            return Content("");

        return Ok(model);
    }

    #endregion
}
