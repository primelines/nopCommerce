using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "AdminHeaderLinks")]
public partial record AdminHeaderLinksDto : BaseNopDto
{

    [JsonProperty("impersonated_customer_name")]
    public string ImpersonatedCustomerName { get; set; }

    [JsonProperty("is_customer_impersonated")]
    public bool IsCustomerImpersonated { get; set; }

    [JsonProperty("display_admin_link")]
    public bool DisplayAdminLink { get; set; }

    [JsonProperty("edit_page_url")]
    public string EditPageUrl { get; set; }
}
