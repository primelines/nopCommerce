using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutShippingAddress")]
public partial record CheckoutShippingAddressDto : BaseNopDto
{
    public CheckoutShippingAddressDto()
    {
        ExistingAddresses = new List<AddressDto>();
        InvalidExistingAddresses = new List<AddressDto>();
        ShippingNewAddress = new AddressDto();
    }


    [JsonProperty("existing_addresses")]
    public IList<AddressDto> ExistingAddresses { get; set; }

    [JsonProperty("invalid_existing_addresses")]
    public IList<AddressDto> InvalidExistingAddresses { get; set; }

    [JsonProperty("shipping_new_address")]
    public AddressDto ShippingNewAddress { get; set; }

    [JsonProperty("new_address_preselected")]
    public bool NewAddressPreselected { get; set; }


    [JsonProperty("selected_billing_address")]
    public int SelectedBillingAddress { get; set; }


    [JsonProperty("display_pickup_in_store")]
    public bool DisplayPickupInStore { get; set; }

    [JsonProperty("pickup_points")]
    public CheckoutPickupPointsDto PickupPoints { get; set; }
}
