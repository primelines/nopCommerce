using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;


public partial record ProductPriceDto : BaseNopDto
{
    /// <summary>
    /// The currency (in 3-letter ISO 4217 format) of the offer price 
    /// </summary>

    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }


    [JsonProperty("old_price")]
    public string OldPrice { get; set; }

    [JsonProperty("old_price_value")]
    public decimal? OldPriceValue { get; set; }


    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("price_value")]
    public decimal PriceValue { get; set; }

    [JsonProperty("price_with_discount")]
    public string PriceWithDiscount { get; set; }

    [JsonProperty("price_with_discount_value")]
    public decimal? PriceWithDiscountValue { get; set; }

    [JsonProperty("product_id")]
    public int ProductId { get; set; }


    [JsonProperty("hide_prices")]
    public bool HidePrices { get; set; }

    /// <summary>
    /// A value indicating whether we should display tax/shipping info (used in Germany)
    /// </summary>

    [JsonProperty("display_tax_shipping_info")]
    public bool DisplayTaxShippingInfo { get; set; }

}

