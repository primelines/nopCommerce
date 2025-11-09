using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;
namespace Nop.Api.DTOs.ShoppingCart;
[JsonObject(Title = "ShoppingCartItemOverview")]
public partial record ShoppingCartItemOverviewDto : BaseNopEntityDto
    {
        public ShoppingCartItemOverviewDto()
        {
            Picture = new PictureDto();
        }


    [JsonProperty("product_id")]
        public int ProductId { get; set; }


    [JsonProperty("product_name")]
        public string ProductName { get; set; }


    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }


    [JsonProperty("quantity")]
        public int Quantity { get; set; }


    [JsonProperty("unit_price")]
        public string UnitPrice { get; set; }

    [JsonProperty("unit_price_value")]
        public decimal UnitPriceValue { get; set; }


    [JsonProperty("attribute_info")]
        public string AttributeInfo { get; set; }


    [JsonProperty("picture")]
        public PictureDto Picture { get; set; }
    }

