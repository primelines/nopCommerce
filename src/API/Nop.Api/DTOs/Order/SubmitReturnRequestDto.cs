using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "SubmitReturnRequest")]
public partial record SubmitReturnRequestDto : BaseNopDto
{
    public SubmitReturnRequestDto()
    {
        Items = new List<OrderItemOverviewDto>();
        AvailableReturnReasons = new List<ReturnRequestReasonDto>();
        AvailableReturnActions = new List<ReturnRequestActionDto>();
    }


    [JsonProperty("order_id")]
    public int OrderId { get; set; }

    [JsonProperty("custom_order_number")]
    public string CustomOrderNumber { get; set; }


    [JsonProperty("items")]
    public IList<OrderItemOverviewDto> Items { get; set; }

    [NopResourceDisplayName("ReturnRequests.ReturnReason")]

    [JsonProperty("return_request_reason_id")]
    public int ReturnRequestReasonId { get; set; }

    [JsonProperty("available_return_reasons")]
    public IList<ReturnRequestReasonDto> AvailableReturnReasons { get; set; }

    [NopResourceDisplayName("ReturnRequests.ReturnAction")]

    [JsonProperty("return_request_action_id")]
    public int ReturnRequestActionId { get; set; }

    [JsonProperty("available_return_actions")]
    public IList<ReturnRequestActionDto> AvailableReturnActions { get; set; }

    [NopResourceDisplayName("ReturnRequests.Comments")]

    [JsonProperty("comments")]
    public string Comments { get; set; }


    [JsonProperty("allow_files")]
    public bool AllowFiles { get; set; }
    [NopResourceDisplayName("ReturnRequests.UploadedFile")]

    [JsonProperty("uploaded_file_guid")]
    public Guid UploadedFileGuid { get; set; }


    [JsonProperty("result")]
    public string Result { get; set; }

#region Nested classes

    public partial record ReturnRequestReasonDto : BaseNopEntityDto
    {

    [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial record ReturnRequestActionDto : BaseNopEntityDto
    {

    [JsonProperty("name")]
        public string Name { get; set; }
    }

    #endregion
}
