using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;
using Nop.Api.DTOs.Videos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductDetails")]
public partial record ProductDetailsDto : BaseNopEntityDto
{
    public ProductDetailsDto()
    {
        DefaultPicture = new PictureDto();
        Pictures = new List<PictureDto>();
        Videos = new List<VideoDto>();
        GiftCard = new GiftCardDto();
        ProductPrice = new ProductPriceDto();
        AddToCart = new AddToCartDto();
        ProductAttributes = new List<ProductAttributeOverviewDto>();
        Vendor = new VendorBriefInfoDto();
        Breadcrumb = new ProductBreadcrumbDto();
        ProductTags = new List<ProductTagDto>();
        ProductSpecification = new ProductSpecificationDto();
        ProductManufacturers = new List<ManufacturerBriefInfoDto>();
        ProductReviewOverview = new ProductReviewOverviewDto();
        ProductReviews = new ProductReviewsDto();
        TierPrices = new List<TierPriceDto>();
        ProductEstimateShipping = new ProductEstimateShippingDto();
    }

    //picture(s)

    [JsonProperty("default_picture_zoom_enabled")]
    public bool DefaultPictureZoomEnabled { get; set; }

    [JsonProperty("default_picture")]
    public PictureDto DefaultPicture { get; set; }

    [JsonProperty("pictures")]
    public IList<PictureDto> Pictures { get; set; }

    //videos

    [JsonProperty("videos")]
    public IList<VideoDto> Videos { get; set; }


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; }

    [JsonProperty("full_description")]
    public string FullDescription { get; set; }

    [JsonProperty("json_ld")]
    public string JsonLd { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("show_sku")]
    public bool ShowSku { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }


    [JsonProperty("show_manufacturer_part_number")]
    public bool ShowManufacturerPartNumber { get; set; }

    [JsonProperty("manufacturer_part_number")]
    public string ManufacturerPartNumber { get; set; }


    [JsonProperty("show_gtin")]
    public bool ShowGtin { get; set; }

    [JsonProperty("gtin")]
    public string Gtin { get; set; }


    [JsonProperty("show_vendor")]
    public bool ShowVendor { get; set; }

    [JsonProperty("vendor")]
    public VendorBriefInfoDto Vendor { get; set; }


    [JsonProperty("gift_card")]
    public GiftCardDto GiftCard { get; set; }


    [JsonProperty("is_ship_enabled")]
    public bool IsShipEnabled { get; set; }

    [JsonProperty("is_free_shipping")]
    public bool IsFreeShipping { get; set; }

    [JsonProperty("free_shipping_notification_enabled")]
    public bool FreeShippingNotificationEnabled { get; set; }

    [JsonProperty("delivery_date")]
    public string DeliveryDate { get; set; }

    [JsonProperty("available_end_date")]
    public DateTime? AvailableEndDate { get; set; }

    [JsonProperty("stock_availability")]
    public string StockAvailability { get; set; }


    [JsonProperty("display_back_in_stock_subscription")]
    public bool DisplayBackInStockSubscription { get; set; }


    [JsonProperty("display_attribute_combination_images_only")]
    public bool DisplayAttributeCombinationImagesOnly { get; set; }


    [JsonProperty("email_a_friend_enabled")]
    public bool EmailAFriendEnabled { get; set; }

    [JsonProperty("compare_products_enabled")]
    public bool CompareProductsEnabled { get; set; }


    [JsonProperty("page_share_code")]
    public string PageShareCode { get; set; }


    [JsonProperty("product_price")]
    public ProductPriceDto ProductPrice { get; set; }


    [JsonProperty("add_to_cart")]
    public AddToCartDto AddToCart { get; set; }


    [JsonProperty("breadcrumb")]
    public ProductBreadcrumbDto Breadcrumb { get; set; }


    [JsonProperty("product_tags")]
    public IList<ProductTagDto> ProductTags { get; set; }


    [JsonProperty("product_attributes")]
    public IList<ProductAttributeOverviewDto> ProductAttributes { get; set; }


    [JsonProperty("product_specification")]
    public ProductSpecificationDto ProductSpecification { get; set; }


    [JsonProperty("product_manufacturers")]
    public IList<ManufacturerBriefInfoDto> ProductManufacturers { get; set; }


    [JsonProperty("product_review_overview")]
    public ProductReviewOverviewDto ProductReviewOverview { get; set; }


    [JsonProperty("product_reviews")]
    public ProductReviewsDto ProductReviews { get; set; }


    [JsonProperty("product_estimate_shipping")]
    public ProductEstimateShippingDto ProductEstimateShipping { get; set; }


    [JsonProperty("tier_prices")]
    public IList<TierPriceDto> TierPrices { get; set; }

    //a list of associated products. For example, "Grouped" products could have several child "simple" products



    [JsonProperty("display_discontinued_message")]
    public bool DisplayDiscontinuedMessage { get; set; }


    [JsonProperty("current_store_name")]
    public string CurrentStoreName { get; set; }


    [JsonProperty("in_stock")]
    public bool InStock { get; set; }

}
