using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;

namespace Nop.Services.Catalog;

/// <summary>
/// Product service
/// </summary>
public partial interface IProductService
{
    #region Products

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteProductAsync(Product product);

    /// <summary>
    /// Delete products
    /// </summary>
    /// <param name="products">Products</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteProductsAsync(IList<Product> products);

    /// <summary>
    /// Gets featured products by a category identifier
    /// </summary>
    /// <param name="categoryId">Category identifier</param>
    /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of featured products
    /// </returns>
    Task<IList<Product>> GetCategoryFeaturedProductsAsync(int categoryId, int storeId = 0);

    /// <summary>
    /// Gets featured products by a manufacturer identifier
    /// </summary>
    /// <param name="manufacturerId">Manufacturer identifier</param>
    /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of featured products
    /// </returns>
    Task<IList<Product>> GetManufacturerFeaturedProductsAsync(int manufacturerId, int storeId = 0);

    /// <summary>
    /// Gets a product
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product
    /// </returns>
    Task<Product> GetProductByIdAsync(int productId);

    /// <summary>
    /// Gets products by identifier
    /// </summary>
    /// <param name="productIds">Product identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    Task<IList<Product>> GetProductsByIdsAsync(int[] productIds);

    /// <summary>
    /// Inserts a product
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertProductAsync(Product product);

    /// <summary>
    /// Inserts products
    /// </summary>
    /// <param name="products">Products to insert</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertProductsAsync(IList<Product> products);

    /// <summary>
    /// Updates the product
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateProductAsync(Product product);

    /// <summary>
    /// Updates products
    /// </summary>
    /// <param name="products">Products to update</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateProductsAsync(IList<Product> products);

    /// <summary>
    /// Get number of product (published and visible) in certain category
    /// </summary>
    /// <param name="categoryIds">Category identifiers</param>
    /// <param name="storeId">Store identifier; 0 to load all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of products
    /// </returns>
    Task<int> GetNumberOfProductsInCategoryAsync(IList<int> categoryIds = null, int storeId = 0);

    /// <summary>
    /// Search products
    /// </summary>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="categoryIds">Category identifiers</param>
    /// <param name="manufacturerIds">Manufacturer identifiers</param>
    /// <param name="storeId">Store identifier; 0 to load all records</param>
    /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
    /// <param name="excludeFeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers); "false" (by default) to load all records; "true" to exclude featured products from results</param>
    /// <param name="priceMin">Minimum price; null to load all records</param>
    /// <param name="priceMax">Maximum price; null to load all records</param>
    /// <param name="productTagId">Product tag identifier; 0 to load all records</param>
    /// <param name="keywords">Keywords</param>
    /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in product descriptions</param>
    /// <param name="searchManufacturerPartNumber">A value indicating whether to search by a specified "keyword" in manufacturer part number</param>
    /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in product SKU</param>
    /// <param name="searchProductTags">A value indicating whether to search by a specified "keyword" in product tags</param>
    /// <param name="languageId">Language identifier (search for text searching)</param>
    /// <param name="filteredSpecOptions">Specification options list to filter products; null to load all records</param>
    /// <param name="orderBy">Order by</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <param name="overridePublished">
    /// null - process "Published" property according to "showHidden" parameter
    /// true - load only "Published" products
    /// false - load only "Unpublished" products
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    Task<IPagedList<Product>> SearchProductsAsync(
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        IList<int> categoryIds = null,
        IList<int> manufacturerIds = null,
        int storeId = 0,
        int vendorId = 0,
        bool excludeFeaturedProducts = false,
        decimal? priceMin = null,
        decimal? priceMax = null,
        int productTagId = 0,
        string keywords = null,
        bool searchDescriptions = false,
        bool searchManufacturerPartNumber = true,
        bool searchSku = true,
        bool searchProductTags = false,
        int languageId = 0,
        IList<SpecificationAttributeOption> filteredSpecOptions = null,
        ProductSortingEnum orderBy = ProductSortingEnum.Position,
        bool showHidden = false,
        bool? overridePublished = null);

    /// <summary>
    /// Gets products by product attribute
    /// </summary>
    /// <param name="productAttributeId">Product attribute identifier</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    Task<IPagedList<Product>> GetProductsByProductAttributeIdAsync(int productAttributeId,
        int pageIndex = 0, int pageSize = int.MaxValue);

    /// <summary>
    /// Get low stock products
    /// </summary>
    /// <param name="vendorId">Vendor identifier; pass null to load all records</param>
    /// <param name="loadPublishedOnly">Whether to load published products only; pass null to load all products, pass true to load only published products, pass false to load only unpublished products</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    Task<IPagedList<Product>> GetLowStockProductsAsync(int? vendorId = null, bool? loadPublishedOnly = true,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Get low stock product combinations
    /// </summary>
    /// <param name="vendorId">Vendor identifier; pass null to load all records</param>
    /// <param name="loadPublishedOnly">Whether to load combinations of published products only; pass null to load all products, pass true to load only published products, pass false to load only unpublished products</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product combinations
    /// </returns>
    Task<IPagedList<ProductAttributeCombination>> GetLowStockProductCombinationsAsync(int? vendorId = null, bool? loadPublishedOnly = true,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    /// <summary>
    /// Gets a product by SKU
    /// </summary>
    /// <param name="sku">SKU</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product
    /// </returns>
    Task<Product> GetProductBySkuAsync(string sku);

    /// <summary>
    /// Gets a products by SKU array
    /// </summary>
    /// <param name="skuArray">SKU array</param>
    /// <param name="vendorId">Vendor ID; 0 to load all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    Task<IList<Product>> GetProductsBySkuAsync(string[] skuArray, int vendorId = 0);

    /// <summary>
    /// Gets number of products by vendor identifier
    /// </summary>
    /// <param name="vendorId">Vendor identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of products
    /// </returns>
    Task<int> GetNumberOfProductsByVendorIdAsync(int vendorId);

    /// <summary>
    /// Get a value indicating whether a product is available now (availability dates)
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="dateTime">Datetime to check; pass null to use current date</param>
    /// <returns>Result</returns>
    bool ProductIsAvailable(Product product, DateTime? dateTime = null);

    /// <summary>
    /// Get a list of allowed quantities (parse 'AllowedQuantities' property)
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>Result</returns>
    int[] ParseAllowedQuantities(Product product);

    /// <summary>
    /// Formats the stock availability/quantity message
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the stock message
    /// </returns>
    Task<string> FormatStockMessageAsync(Product product);

    /// <summary>
    /// Formats SKU
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="attributesXml">Attributes in XML format</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the sKU
    /// </returns>
    Task<string> FormatSkuAsync(Product product, string attributesXml = null);

    /// <summary>
    /// Formats manufacturer part number
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="attributesXml">Attributes in XML format</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer part number
    /// </returns>
    Task<string> FormatMpnAsync(Product product, string attributesXml = null);

    /// <summary>
    /// Formats GTIN
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="attributesXml">Attributes in XML format</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the gTIN
    /// </returns>
    Task<string> FormatGtinAsync(Product product, string attributesXml = null);


    /// <summary>
    /// Returns a list of sku of not existing products
    /// </summary>
    /// <param name="productSku">The sku of the products to check</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of sku not existing products
    /// </returns>
    Task<string[]> GetNotExistingProductsAsync(string[] productSku);

    #endregion

    #region Inventory management methods

    /// <summary>
    /// Adjust inventory
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="quantityToChange">Quantity to increase or decrease</param>
    /// <param name="attributesXml">Attributes in XML format</param>
    /// <param name="message">Message for the stock quantity history</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AdjustInventoryAsync(Product product, int quantityToChange, string attributesXml = "", string message = "");


    #endregion

    #region Tier prices

    /// <summary>
    /// Gets a product tier prices for customer
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="customer">Customer</param>
    /// <param name="store">Store</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<IList<TierPrice>> GetTierPricesAsync(Product product, Customer customer, Store store);

    /// <summary>
    /// Gets a tier prices by product identifier
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<IList<TierPrice>> GetTierPricesByProductAsync(int productId);

    /// <summary>
    /// Deletes a tier price
    /// </summary>
    /// <param name="tierPrice">Tier price</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteTierPriceAsync(TierPrice tierPrice);

    /// <summary>
    /// Gets a tier price
    /// </summary>
    /// <param name="tierPriceId">Tier price identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the ier price
    /// </returns>
    Task<TierPrice> GetTierPriceByIdAsync(int tierPriceId);

    /// <summary>
    /// Inserts a tier price
    /// </summary>
    /// <param name="tierPrice">Tier price</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertTierPriceAsync(TierPrice tierPrice);

    /// <summary>
    /// Updates the tier price
    /// </summary>
    /// <param name="tierPrice">Tier price</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateTierPriceAsync(TierPrice tierPrice);

    /// <summary>
    /// Gets a preferred tier price
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="customer">Customer</param>
    /// <param name="store">Store</param>
    /// <param name="quantity">Quantity</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the ier price
    /// </returns>
    Task<TierPrice> GetPreferredTierPriceAsync(Product product, Customer customer, Store store, int quantity);

    #endregion

    #region Product pictures

    /// <summary>
    /// Deletes a product picture
    /// </summary>
    /// <param name="productPicture">Product picture</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteProductPictureAsync(ProductPicture productPicture);

    /// <summary>
    /// Gets a product pictures by product identifier
    /// </summary>
    /// <param name="productId">The product identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product pictures
    /// </returns>
    Task<IList<ProductPicture>> GetProductPicturesByProductIdAsync(int productId);

    /// <summary>
    /// Gets a product picture
    /// </summary>
    /// <param name="productPictureId">Product picture identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product picture
    /// </returns>
    Task<ProductPicture> GetProductPictureByIdAsync(int productPictureId);

    /// <summary>
    /// Inserts a product picture
    /// </summary>
    /// <param name="productPicture">Product picture</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertProductPictureAsync(ProductPicture productPicture);

    /// <summary>
    /// Updates a product picture
    /// </summary>
    /// <param name="productPicture">Product picture</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateProductPictureAsync(ProductPicture productPicture);

    /// <summary>
    /// Get the IDs of all product images 
    /// </summary>
    /// <param name="productsIds">Products IDs</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the all picture identifiers grouped by product ID
    /// </returns>
    Task<IDictionary<int, int[]>> GetProductsImagesIdsAsync(int[] productsIds);

    /// <summary>
    /// Get products to which a discount is applied
    /// </summary>
    /// <param name="discountId">Discount identifier; pass null to load all records</param>
    /// <param name="showHidden">A value indicating whether to load deleted products</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of products
    /// </returns>
    Task<IPagedList<Product>> GetProductsWithAppliedDiscountAsync(int? discountId = null,
        bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue);

    #endregion

    #region Product videos

    /// <summary>
    /// Deletes a product video
    /// </summary>
    /// <param name="productVideo">Product video</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteProductVideoAsync(ProductVideo productVideo);

    /// <summary>
    /// Gets a product videos by product identifier
    /// </summary>
    /// <param name="productId">The product identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product videos
    /// </returns>
    Task<IList<ProductVideo>> GetProductVideosByProductIdAsync(int productId);

    /// <summary>
    /// Gets a product video
    /// </summary>
    /// <param name="productPictureId">Product video identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product video
    /// </returns>
    Task<ProductVideo> GetProductVideoByIdAsync(int productVideoId);

    /// <summary>
    /// Inserts a product video
    /// </summary>
    /// <param name="productVideo">Product picture</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertProductVideoAsync(ProductVideo productVideo);

    /// <summary>
    /// Updates a product video
    /// </summary>
    /// <param name="productVideo">Product video</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateProductVideoAsync(ProductVideo productVideo);

    #endregion

    #region Stock quantity history

    /// <summary>
    /// Add stock quantity change entry
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="quantityAdjustment">Quantity adjustment</param>
    /// <param name="stockQuantity">Current stock quantity</param>
    /// <param name="message">Message</param>
    /// <param name="combinationId">Product attribute combination identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddStockQuantityHistoryEntryAsync(Product product, int quantityAdjustment, int stockQuantity, string message = "", int? combinationId = null);

    /// <summary>
    /// Get the history of the product stock quantity changes
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="combinationId">Product attribute combination identifier; pass 0 to load all entries</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of stock quantity change entries
    /// </returns>
    Task<IPagedList<StockQuantityHistory>> GetStockQuantityHistoryAsync(Product product, int combinationId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue);

    #endregion

    #region Product discounts

    /// <summary>
    /// Clean up product references for a specified discount
    /// </summary>
    /// <param name="discount">Discount</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task ClearDiscountProductMappingAsync(Discount discount);

    /// <summary>
    /// Get a discount-product mapping records by product identifier
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<IList<DiscountProductMapping>> GetAllDiscountsAppliedToProductAsync(int productId);

    /// <summary>
    /// Get a discount-product mapping record
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <param name="discountId">Discount identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    Task<DiscountProductMapping> GetDiscountAppliedToProductAsync(int productId, int discountId);

    /// <summary>
    /// Inserts a discount-product mapping record
    /// </summary>
    /// <param name="discountProductMapping">Discount-product mapping</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertDiscountProductMappingAsync(DiscountProductMapping discountProductMapping);

    /// <summary>
    /// Deletes a discount-product mapping record
    /// </summary>
    /// <param name="discountProductMapping">Discount-product mapping</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteDiscountProductMappingAsync(DiscountProductMapping discountProductMapping);

    #endregion
}