using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.Settings;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Models.Translation;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a product model
/// </summary>
public partial record ProductModel : BaseNopEntityModel,
    IAclSupportedModel, IDiscountSupportedModel, ITranslationSupportedModel, ILocalizedModel<ProductLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public ProductModel()
    {
        ProductPictureModels = new List<ProductPictureModel>();
        ProductVideoModels = new List<ProductVideoModel>();
        Locales = new List<ProductLocalizedModel>();
        CopyProductModel = new CopyProductModel();
        AddPictureModel = new ProductPictureModel();
        AddVideoModel = new ProductVideoModel();
        ProductEditorSettingsModel = new ProductEditorSettingsModel();
        StockQuantityHistory = new StockQuantityHistoryModel();
        AvailableProductTemplates = new List<SelectListItem>();
        AvailableTaxCategories = new List<SelectListItem>();
        AvailableDeliveryDates = new List<SelectListItem>();
        AvailableProductAvailabilityRanges = new List<SelectListItem>();

        AvailableVendors = new List<SelectListItem>();

        SelectedStoreIds = new List<int>();
        AvailableStores = new List<SelectListItem>();

        SelectedManufacturerIds = new List<int>();
        AvailableManufacturers = new List<SelectListItem>();

        SelectedCategoryIds = new List<int>();
        AvailableCategories = new List<SelectListItem>();

        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();

        SelectedDiscountIds = new List<int>();
        AvailableDiscounts = new List<SelectListItem>();

        AvailableProductTags = new List<SelectListItem>();
        SelectedProductTags = new List<string>();

        RelatedProductSearchModel = new RelatedProductSearchModel();
        CrossSellProductSearchModel = new CrossSellProductSearchModel();
        FilterLevelValueSearchModel = new FilterLevelValueSearchModel();
        ProductPictureSearchModel = new ProductPictureSearchModel();
        ProductVideoSearchModel = new ProductVideoSearchModel();
        ProductSpecificationAttributeSearchModel = new ProductSpecificationAttributeSearchModel();
        ProductOrderSearchModel = new ProductOrderSearchModel();
        TierPriceSearchModel = new TierPriceSearchModel();
        StockQuantityHistorySearchModel = new StockQuantityHistorySearchModel();
        ProductAttributeMappingSearchModel = new ProductAttributeMappingSearchModel();
        ProductAttributeCombinationSearchModel = new ProductAttributeCombinationSearchModel();
    }

    #endregion

    #region Properties

    //picture thumbnail
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.PictureThumbnailUrl")]
    public string PictureThumbnailUrl { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProductTemplate")]
    public int ProductTemplateId { get; set; }
    public IList<SelectListItem> AvailableProductTemplates { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
    public string ShortDescription { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.FullDescription")]
    public string FullDescription { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.SeName")]
    public string SeName { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AllowCustomerReviews")]
    public bool AllowCustomerReviews { get; set; }

    public IList<SelectListItem> AvailableProductTags { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProductTags")]
    public IList<string> SelectedProductTags { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Sku")]
    public string Sku { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ManufacturerPartNumber")]
    public string ManufacturerPartNumber { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.GTIN")]
    public virtual string Gtin { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.IsShipEnabled")]
    public bool IsShipEnabled { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.IsFreeShipping")]
    public bool IsFreeShipping { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ShipSeparately")]
    public bool ShipSeparately { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AdditionalShippingCharge")]
    public decimal AdditionalShippingCharge { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DeliveryDate")]
    public int DeliveryDateId { get; set; }
    public IList<SelectListItem> AvailableDeliveryDates { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.IsTaxExempt")]
    public bool IsTaxExempt { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.TaxCategory")]
    public int TaxCategoryId { get; set; }
    public IList<SelectListItem> AvailableTaxCategories { get; set; }


    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProductAvailabilityRange")]
    public int ProductAvailabilityRangeId { get; set; }
    public IList<SelectListItem> AvailableProductAvailabilityRanges { get; set; }


    [NopResourceDisplayName("Admin.Catalog.Products.Fields.StockQuantity")]
    public int StockQuantity { get; set; }

    public int LastStockQuantity { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.StockQuantity")]
    public string StockQuantityStr { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DisplayStockAvailability")]
    public bool DisplayStockAvailability { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DisplayStockQuantity")]
    public bool DisplayStockQuantity { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.MinStockQuantity")]
    public int MinStockQuantity { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.LowStockActivity")]
    public int LowStockActivityId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.NotifyAdminForQuantityBelow")]
    public int NotifyAdminForQuantityBelow { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.BackorderMode")]
    public int BackorderModeId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AllowBackInStockSubscriptions")]
    public bool AllowBackInStockSubscriptions { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.OrderMinimumQuantity")]
    public int OrderMinimumQuantity { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.OrderMaximumQuantity")]
    public int OrderMaximumQuantity { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AllowedQuantities")]
    public string AllowedQuantities { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DisplayAttributeCombinationImagesOnly")]
    public bool DisplayAttributeCombinationImagesOnly { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.NotReturnable")]
    public bool NotReturnable { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DisableBuyButton")]
    public bool DisableBuyButton { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.DisableWishlistButton")]
    public bool DisableWishlistButton { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AvailableForPreOrder")]
    public bool AvailableForPreOrder { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.PreOrderAvailabilityStartDateTimeUtc")]
    [UIHint("DateTimeNullable")]
    public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AgeVerification")]
    public bool AgeVerification { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.MinimumAgeToPurchase")]
    public int MinimumAgeToPurchase { get; set; }


    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Price")]
    public decimal Price { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Price")]
    public string FormattedPrice { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.OldPrice")]
    public decimal OldPrice { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Weight")]
    public decimal Weight { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Length")]
    public decimal Length { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Width")]
    public decimal Width { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Height")]
    public decimal Height { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AvailableStartDateTime")]
    [UIHint("DateTimeNullable")]
    public DateTime? AvailableStartDateTimeUtc { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.AvailableEndDateTime")]
    [UIHint("DateTimeNullable")]
    public DateTime? AvailableEndDateTimeUtc { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Published")]
    public bool Published { get; set; }

    public string PrimaryStoreCurrencyCode { get; set; }

    public string BaseDimensionIn { get; set; }

    public string BaseWeightIn { get; set; }

    public IList<ProductLocalizedModel> Locales { get; set; }

    public IList<int> SelectedCustomerRoleIds { get; set; }
    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    //store mapping
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.LimitedToStores")]
    public IList<int> SelectedStoreIds { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }

    //categories
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Categories")]
    public IList<int> SelectedCategoryIds { get; set; }
    public IList<SelectListItem> AvailableCategories { get; set; }

    //manufacturers
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Manufacturers")]
    public IList<int> SelectedManufacturerIds { get; set; }
    public IList<SelectListItem> AvailableManufacturers { get; set; }

    //vendors
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Vendor")]
    public int VendorId { get; set; }
    public IList<SelectListItem> AvailableVendors { get; set; }

    //discounts
    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Discounts")]
    public IList<int> SelectedDiscountIds { get; set; }
    public IList<SelectListItem> AvailableDiscounts { get; set; }

    //vendor
    public bool IsLoggedInAsVendor { get; set; }

    //pictures
    public ProductPictureModel AddPictureModel { get; set; }
    public IList<ProductPictureModel> ProductPictureModels { get; set; }

    //videos
    public ProductVideoModel AddVideoModel { get; set; }
    public IList<ProductVideoModel> ProductVideoModels { get; set; }

    //product attributes
    public bool ProductAttributesExist { get; set; }
    public bool CanCreateCombinations { get; set; }

    //specification attributes
    public bool HasAvailableSpecificationAttributes { get; set; }

    public bool PreTranslationAvailable { get; set; }

    //copy product
    public CopyProductModel CopyProductModel { get; set; }

    //editor settings
    public ProductEditorSettingsModel ProductEditorSettingsModel { get; set; }

    //stock quantity history
    public StockQuantityHistoryModel StockQuantityHistory { get; set; }

    public RelatedProductSearchModel RelatedProductSearchModel { get; set; }

    public CrossSellProductSearchModel CrossSellProductSearchModel { get; set; }

    public FilterLevelValueSearchModel FilterLevelValueSearchModel { get; set; }

    public ProductPictureSearchModel ProductPictureSearchModel { get; set; }

    public ProductVideoSearchModel ProductVideoSearchModel { get; set; }

    public ProductSpecificationAttributeSearchModel ProductSpecificationAttributeSearchModel { get; set; }

    public ProductOrderSearchModel ProductOrderSearchModel { get; set; }

    public TierPriceSearchModel TierPriceSearchModel { get; set; }

    public StockQuantityHistorySearchModel StockQuantityHistorySearchModel { get; set; }

    public ProductAttributeMappingSearchModel ProductAttributeMappingSearchModel { get; set; }

    public ProductAttributeCombinationSearchModel ProductAttributeCombinationSearchModel { get; set; }

    #endregion
}

public partial record ProductLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
    public string ShortDescription { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.FullDescription")]
    public string FullDescription { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.Fields.SeName")]
    public string SeName { get; set; }
}