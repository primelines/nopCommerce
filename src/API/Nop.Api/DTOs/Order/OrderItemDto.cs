using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Order;
[JsonObject(Title = "OrderItem")]
public partial record OrderItemDto : BaseNopEntityDto
{
    public OrderItemDto()
    {
        Picture = new PictureDto();
    }


    [JsonProperty("order_item_guid")]
    public Guid OrderItemGuid { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("product_id")]
    public int ProductId { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
    public string ProductSeName { get; set; }

    [JsonProperty("unit_price")]
    public string UnitPrice { get; set; }

    [JsonProperty("unit_price_value")]
    public decimal UnitPriceValue { get; set; }

    [JsonProperty("sub_total")]
    public string SubTotal { get; set; }

    [JsonProperty("sub_total_value")]
    public decimal SubTotalValue { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("picture")]
    public PictureDto Picture { get; set; }

    [JsonProperty("attribute_info")]
    public string AttributeInfo { get; set; }

    [JsonProperty("rental_info")]
    public string RentalInfo { get; set; }


    [JsonProperty("vendor_name")]
    public string VendorName { get; set; }

    //downloadable product properties

    [JsonProperty("download_id")]
    public int DownloadId { get; set; }

    [JsonProperty("license_id")]
    public int LicenseId { get; set; }
}
