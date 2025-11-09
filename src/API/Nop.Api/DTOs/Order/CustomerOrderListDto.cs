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
        RecurringOrders = new List<RecurringOrderDto>();
        RecurringPaymentErrors = new List<string>();
    }


    [JsonProperty("orders")]
    public IList<CustomerOrderDetailsDto> Orders { get; set; }

    [JsonProperty("recurring_orders")]
    public IList<RecurringOrderDto> RecurringOrders { get; set; }

    [JsonProperty("recurring_payment_errors")]
    public IList<string> RecurringPaymentErrors { get; set; }

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

    public partial record RecurringOrderDto : BaseNopEntityDto
    {

    [JsonProperty("start_date")]
        public string StartDate { get; set; }

    [JsonProperty("cycle_info")]
        public string CycleInfo { get; set; }

    [JsonProperty("next_payment")]
        public string NextPayment { get; set; }

    [JsonProperty("total_cycles")]
        public int TotalCycles { get; set; }

    [JsonProperty("cycles_remaining")]
        public int CyclesRemaining { get; set; }

    [JsonProperty("initial_order_id")]
        public int InitialOrderId { get; set; }

    [JsonProperty("can_retry_last_payment")]
        public bool CanRetryLastPayment { get; set; }

    [JsonProperty("initial_order_number")]
        public string InitialOrderNumber { get; set; }

    [JsonProperty("can_cancel")]
        public bool CanCancel { get; set; }
    }

    #endregion
}
