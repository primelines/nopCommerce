using System.Xml.Linq;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Api.DTOs.Catalog;

namespace Nop.Api.Factories;

public partial interface ICatalogDtoFactory
{
    #region Categories

    /// <summary>
    /// Prepare category model
    /// </summary>
    /// <param name="category">Category</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the category model
    /// </returns>
    Task<CategoryDto> PrepareCategoryDtoAsync(Category category, CatalogProductsCommand command);

    /// <summary>
    /// Prepare category template view path
    /// </summary>
    /// <param name="templateId">Template identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the category template view path
    /// </returns>
    Task<string> PrepareCategoryTemplateViewPathAsync(int templateId);

    /// <summary>
    /// Prepare category navigation model
    /// </summary>
    /// <param name="currentCategoryId">Current category identifier</param>
    /// <param name="currentProductId">Current product identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the category navigation model
    /// </returns>
    Task<CategoryNavigationDto> PrepareCategoryNavigationDtoAsync(int currentCategoryId,
        int currentProductId);

    ///// <summary>
    ///// Prepare top menu model
    ///// </summary>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the op menu model
    ///// </returns>
    //Task<TopMenuDto> PrepareTopMenuDtoAsync();

    /// <summary>
    /// Prepare homepage category models
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of homepage category models
    /// </returns>
    Task<List<CategoryDto>> PrepareHomepageCategoryDtosAsync();

    /// <summary>
    /// Prepare root categories for menu
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of category (simple) models
    /// </returns>
    Task<List<CategorySimpleDto>> PrepareRootCategoriesAsync();

    /// <summary>
    /// Prepare subcategories for menu
    /// </summary>
    /// <param name="id">Id of category to get subcategory</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the 
    /// </returns>
    Task<List<CategorySimpleDto>> PrepareSubCategoriesAsync(int id);

    /// <summary>
    /// Prepares the category products model
    /// </summary>
    /// <param name="category">Category</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the category products model
    /// </returns>
    Task<CatalogProductsDto> PrepareCategoryProductsModelAsync(Category category, CatalogProductsCommand command);

    /// <summary>
    /// Prepare category (simple) models
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of category (simple) models
    /// </returns>
    Task<List<CategorySimpleDto>> PrepareCategorySimpleDtosAsync();

    /// <summary>
    /// Prepare category (simple) models
    /// </summary>
    /// <param name="rootCategoryId">Root category identifier</param>
    /// <param name="loadSubCategories">A value indicating whether subcategories should be loaded</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of category (simple) models
    /// </returns>
    Task<List<CategorySimpleDto>> PrepareCategorySimpleDtosAsync(int rootCategoryId, bool loadSubCategories = true);

    /// <summary>
    /// Prepare category (simple) xml document
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the xml document of category (simple) models
    /// </returns>
    Task<XDocument> PrepareCategoryXmlDocumentAsync();

    #endregion

    #region Manufacturers

    /// <summary>
    /// Prepare manufacturer model
    /// </summary>
    /// <param name="manufacturer">Manufacturer identifier</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer model
    /// </returns>
    Task<ManufacturerDto> PrepareManufacturerDtoAsync(Manufacturer manufacturer, CatalogProductsCommand command);

    /// <summary>
    /// Prepares the manufacturer products model
    /// </summary>
    /// <param name="manufacturer">Manufacturer</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer products model
    /// </returns>
    Task<CatalogProductsDto> PrepareManufacturerProductsModelAsync(Manufacturer manufacturer, CatalogProductsCommand command);

    /// <summary>
    /// Prepare manufacturer template view path
    /// </summary>
    /// <param name="templateId">Template identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer template view path
    /// </returns>
    Task<string> PrepareManufacturerTemplateViewPathAsync(int templateId);

    /// <summary>
    /// Prepare manufacturer all models
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of manufacturer models
    /// </returns>
    Task<List<ManufacturerDto>> PrepareManufacturerAllModelsAsync();

    /// <summary>
    /// Prepare manufacturer navigation model
    /// </summary>
    /// <param name="currentManufacturerId">Current manufacturer identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer navigation model
    /// </returns>
    Task<ManufacturerNavigationDto> PrepareManufacturerNavigationDtoAsync(int currentManufacturerId);

    #endregion

    #region Vendors

    /// <summary>
    /// Prepare vendor model
    /// </summary>
    /// <param name="vendor">Vendor</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the vendor model
    /// </returns>
    Task<VendorDto> PrepareVendorDtoAsync(Vendor vendor, CatalogProductsCommand command);

    /// <summary>
    /// Prepares the vendor products model
    /// </summary>
    /// <param name="vendor">Vendor</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the vendor products model
    /// </returns>
    Task<CatalogProductsDto> PrepareVendorProductsModelAsync(Vendor vendor, CatalogProductsCommand command);

    /// <summary>
    /// Prepare vendor all models
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of vendor models
    /// </returns>
    Task<List<VendorDto>> PrepareVendorAllModelsAsync();

    /// <summary>
    /// Prepare vendor navigation model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the vendor navigation model
    /// </returns>
    Task<VendorNavigationDto> PrepareVendorNavigationDtoAsync();

    #endregion

    #region Product tags

    /// <summary>
    /// Prepare popular product tags model
    /// </summary>
    /// <param name="numberTagsToReturn">The number of tags to be returned; pass 0 to get all tags</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product tags model
    /// </returns>
    Task<PopularProductTagsDto> PreparePopularProductTagsDtoAsync(int numberTagsToReturn = 0);

    /// <summary>
    /// Prepare products by tag model
    /// </summary>
    /// <param name="productTag">Product tag</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products by tag model
    /// </returns>
    Task<ProductsByTagDto> PrepareProductsByTagDtoAsync(ProductTag productTag, CatalogProductsCommand command);

    /// <summary>
    /// Prepares the tag products model
    /// </summary>
    /// <param name="productTag">Product tag</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the ag products model
    /// </returns>
    Task<CatalogProductsDto> PrepareTagProductsModelAsync(ProductTag productTag, CatalogProductsCommand command);

    #endregion

    #region Searching

    /// <summary>
    /// Prepare search model
    /// </summary>
    /// <param name="model">Search model</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the search model
    /// </returns>
    Task<CatalogSearchDto> PrepareSearchDtoAsync(CatalogSearchDto model, CatalogProductsCommand command);

    /// <summary>
    /// Prepares the search products model
    /// </summary>
    /// <param name="model">Search model</param>
    /// <param name="command">Model to get the catalog products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the search products model
    /// </returns>
    Task<CatalogProductsDto> PrepareSearchProductsModelAsync(CatalogSearchDto searchModel, CatalogProductsCommand command);

    /// <summary>
    /// Prepare search box model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the search box model
    /// </returns>
    Task<SearchBoxDto> PrepareSearchBoxDtoAsync();

    #endregion

    #region Common

    /// <summary>
    /// Prepare sorting options
    /// </summary>
    /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
    /// <param name="command">Catalog paging filtering command</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task PrepareSortingOptionsAsync(CatalogProductsDto pagingFilteringModel, CatalogProductsCommand command);

    /// <summary>
    /// Prepare view modes
    /// </summary>
    /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
    /// <param name="command">Catalog paging filtering command</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task PrepareViewModesAsync(CatalogProductsDto pagingFilteringModel, CatalogProductsCommand command);
    #endregion
}