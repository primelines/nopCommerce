using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "CustomerReturnRequests")]
public partial record CustomerReturnRequestsDto : BaseNopDto
{
    public CustomerReturnRequestsDto()
    {
        Items = new List<ReturnRequestDto>();
    }


    [JsonProperty("items")]
    public IList<ReturnRequestDto> Items { get; set; }

    #region Nested classes

    public partial record ReturnRequestDto : BaseNopEntityDto
    {

    [JsonProperty("custom_number")]
        public string CustomNumber { get; set; }

    [JsonProperty("return_request_status")]
        public string ReturnRequestStatus { get; set; }

    [JsonProperty("product_id")]
        public int ProductId { get; set; }

    [JsonProperty("product_name")]
        public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }

    [JsonProperty("quantity")]
        public int Quantity { get; set; }


    [JsonProperty("return_reason")]
        public string ReturnReason { get; set; }

    [JsonProperty("return_action")]
        public string ReturnAction { get; set; }

    [JsonProperty("comments")]
        public string Comments { get; set; }

    [JsonProperty("uploaded_file_guid")]
        public Guid UploadedFileGuid { get; set; }


    [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }

    #endregion
}
