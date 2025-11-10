using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.ShoppingCart;

[JsonObject(Title = "ShoppingCartItem")]
public partial record ShoppingCartItemDto : BaseNopEntityDto
{
    public ShoppingCartItemDto()
    {
        Picture = new PictureDto();
        AllowedQuantities = new List<SelectListItemDto>();
        Warnings = new List<string>();
    }


    [JsonProperty("sku")]
    public string Sku { get; set; }


    [JsonProperty("vendor_name")]
    public string VendorName { get; set; }


    [JsonProperty("picture")]
    public PictureDto Picture { get; set; }


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


    [JsonProperty("discount")]
    public string Discount { get; set; }

    [JsonProperty("discount_value")]
    public decimal DiscountValue { get; set; }

    [JsonProperty("maximum_discounted_qty")]
    public int? MaximumDiscountedQty { get; set; }


    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("allowed_quantities")]
    public List<SelectListItemDto> AllowedQuantities { get; set; }


    [JsonProperty("attribute_info")]
    public string AttributeInfo { get; set; }

    [JsonProperty("allow_item_editing")]
    public bool AllowItemEditing { get; set; }


    [JsonProperty("disable_removal")]
    public bool DisableRemoval { get; set; }


    [JsonProperty("warnings")]
    public IList<string> Warnings { get; set; }
}
