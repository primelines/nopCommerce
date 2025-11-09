using Newtonsoft.Json;
using Nop.Core.Domain.Orders;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "CustomerOrderList")]
public partial record CustomerOrderListDto : BaseNopDto
{
    public CustomerOrderListDto()
    {
        Orders = new List<CustomerOrderDetailsDto>();
    }


    [JsonProperty("orders")]
    public IList<CustomerOrderDetailsDto> Orders { get; set; }

    #region Nested classes

    public partial record CustomerOrderDetailsDto : BaseNopEntityDto
    {

    [JsonProperty("custom_order_number")]
        public string CustomOrderNumber { get; set; }

    [JsonProperty("order_total")]
        public string OrderTotal { get; set; }

    [JsonProperty("is_return_request_allowed")]
        public bool IsReturnRequestAllowed { get; set; }

    [JsonProperty("order_status_enum")]
        public OrderStatus OrderStatusEnum { get; set; }

    [JsonProperty("order_status")]
        public string OrderStatus { get; set; }

    [JsonProperty("payment_status")]
        public string PaymentStatus { get; set; }

    [JsonProperty("shipping_status")]
        public string ShippingStatus { get; set; }

    [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }

    #endregion
}
