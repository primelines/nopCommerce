using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;


[JsonObject(Title = "ProductPriceOverview")]
public partial record ProductPriceOverviewDto : BaseNopDto
    {

        [JsonProperty("old_price")]
        public string OldPrice { get; set; }

        [JsonProperty("old_price_value")]
        public decimal? OldPriceValue { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("price_value")]
        public decimal? PriceValue { get; set; }

        [JsonProperty("disable_buy_button")]
        public bool DisableBuyButton { get; set; }

        [JsonProperty("disable_wishlist_button")]
        public bool DisableWishlistButton { get; set; }

        [JsonProperty("disable_add_to_compare_list_button")]
        public bool DisableAddToCompareListButton { get; set; }


        [JsonProperty("available_for_pre_order")]
        public bool AvailableForPreOrder { get; set; }

        [JsonProperty("pre_order_availability_start_date_time_utc")]
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        [JsonProperty("force_redirection_after_adding_to_cart")]
        public bool ForceRedirectionAfterAddingToCart { get; set; }

        /// <summary>
        /// A value indicating whether we should display tax/shipping info (used in Germany)
        /// </summary>

        [JsonProperty("display_tax_shipping_info")]
        public bool DisplayTaxShippingInfo { get; set; }
    }


