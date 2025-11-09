using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;


    #region Nested classes

    public partial record OrderItemOverviewDto : BaseNopEntityDto
    {

    [JsonProperty("product_id")]
        public int ProductId { get; set; }


    [JsonProperty("product_name")]
        public string ProductName { get; set; }


    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }


    [JsonProperty("attribute_info")]
        public string AttributeInfo { get; set; }


    [JsonProperty("unit_price")]
        public string UnitPrice { get; set; }


    [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    #endregion
