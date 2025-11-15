using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Footer")]
public partial record FooterDto : BaseNopDto
{
    public FooterDto()
    {
        Topics = new List<FooterTopicDto>();
    }


    [JsonProperty("store_name")]
    public string StoreName { get; set; }

    [JsonProperty("is_home_page")]
    public bool IsHomePage { get; set; }

    [JsonProperty("shopping_cart_enabled")]
    public bool ShoppingCartEnabled { get; set; }

    [JsonProperty("sitemap_enabled")]
    public bool SitemapEnabled { get; set; }

    [JsonProperty("search_enabled")]
    public bool SearchEnabled { get; set; }

    [JsonProperty("news_enabled")]
    public bool NewsEnabled { get; set; }

    [JsonProperty("blog_enabled")]
    public bool BlogEnabled { get; set; }

    [JsonProperty("forum_enabled")]
    public bool ForumEnabled { get; set; }

    [JsonProperty("recently_viewed_products_enabled")]
    public bool RecentlyViewedProductsEnabled { get; set; }

    [JsonProperty("new_products_enabled")]
    public bool NewProductsEnabled { get; set; }

    [JsonProperty("allow_customers_to_apply_for_vendor_account")]
    public bool AllowCustomersToApplyForVendorAccount { get; set; }

    [JsonProperty("allow_customers_to_check_gift_card_balance")]
    public bool AllowCustomersToCheckGiftCardBalance { get; set; }

    [JsonProperty("display_tax_shipping_info_footer")]
    public bool DisplayTaxShippingInfoFooter { get; set; }

    [JsonProperty("hide_powered_by_nop_commerce")]
    public bool HidePoweredByNopCommerce { get; set; }


    [JsonProperty("working_language_id")]
    public int WorkingLanguageId { get; set; }


    [JsonProperty("topics")]
    public IList<FooterTopicDto> Topics { get; set; }


    [JsonProperty("display_sitemap_footer_item")]
    public bool DisplaySitemapFooterItem { get; set; }

    [JsonProperty("display_contact_us_footer_item")]
    public bool DisplayContactUsFooterItem { get; set; }

    [JsonProperty("display_product_search_footer_item")]
    public bool DisplayProductSearchFooterItem { get; set; }

    [JsonProperty("display_news_footer_item")]
    public bool DisplayNewsFooterItem { get; set; }

    [JsonProperty("display_blog_footer_item")]
    public bool DisplayBlogFooterItem { get; set; }

    [JsonProperty("display_forums_footer_item")]
    public bool DisplayForumsFooterItem { get; set; }

    [JsonProperty("display_recently_viewed_products_footer_item")]
    public bool DisplayRecentlyViewedProductsFooterItem { get; set; }

    [JsonProperty("display_new_products_footer_item")]
    public bool DisplayNewProductsFooterItem { get; set; }

    [JsonProperty("display_customer_info_footer_item")]
    public bool DisplayCustomerInfoFooterItem { get; set; }

    [JsonProperty("display_customer_orders_footer_item")]
    public bool DisplayCustomerOrdersFooterItem { get; set; }

    [JsonProperty("display_customer_addresses_footer_item")]
    public bool DisplayCustomerAddressesFooterItem { get; set; }

    [JsonProperty("display_shopping_cart_footer_item")]
    public bool DisplayShoppingCartFooterItem { get; set; }


    [JsonProperty("display_apply_vendor_account_footer_item")]
    public bool DisplayApplyVendorAccountFooterItem { get; set; }

    #region Nested classes

    public partial record FooterTopicDto : BaseNopEntityDto
    {

    [JsonProperty("name")]
        public string Name { get; set; }

    [JsonProperty("se_name")]
        public string SeName { get; set; }


    [JsonProperty("include_in_footer_column1")]
        public bool IncludeInFooterColumn1 { get; set; }

    [JsonProperty("include_in_footer_column2")]
        public bool IncludeInFooterColumn2 { get; set; }

    [JsonProperty("include_in_footer_column3")]
        public bool IncludeInFooterColumn3 { get; set; }
    }

    #endregion
}
