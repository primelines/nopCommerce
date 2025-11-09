using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutBillingAddressEditAddressResponse")]
public partial record CheckoutBillingAddressDtoEditAddressResponse : BaseNopDto
{


    [JsonProperty("redirect")]
    public string Redirect { get; set; }

    [JsonProperty("selected_id")]
    public int SelectedId { get; set; }

    [JsonProperty("update_section")]
    public CheckoutBillingAddressDtoUpdateSectionJsonDto UpdateSection { get; set; }


}
