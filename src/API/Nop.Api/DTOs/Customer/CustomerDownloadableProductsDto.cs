using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerDownloadableProducts")]
public partial record CustomerDownloadableProductsDto : BaseNopDto
{
    public CustomerDownloadableProductsDto()
    {
        Items = new List<DownloadableProductsModel>();
    }


    [JsonProperty("items")]
    public IList<DownloadableProductsModel> Items { get; set; }

    #region Nested classes

    public partial record DownloadableProductsModel : BaseNopDto
    {

    [JsonProperty("order_item_guid")]
        public Guid OrderItemGuid { get; set; }


    [JsonProperty("order_id")]
        public int OrderId { get; set; }

    [JsonProperty("custom_order_number")]
        public string CustomOrderNumber { get; set; }


    [JsonProperty("product_id")]
        public int ProductId { get; set; }

    [JsonProperty("product_name")]
        public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }

    [JsonProperty("product_attributes")]
        public string ProductAttributes { get; set; }


    [JsonProperty("download_id")]
        public int DownloadId { get; set; }

    [JsonProperty("license_id")]
        public int LicenseId { get; set; }


    [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }

    #endregion
}

[JsonObject(Title = "UserAgreement")]
public partial record UserAgreementDto : BaseNopDto
{

    [JsonProperty("order_item_guid")]
    public Guid OrderItemGuid { get; set; }

    [JsonProperty("user_agreement_text")]
    public string UserAgreementText { get; set; }
}
