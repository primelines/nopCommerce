using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutBillingAddress")]
public partial record CheckoutBillingAddressDto : BaseNopDto
{
    public CheckoutBillingAddressDto()
    {
        ExistingAddresses = new List<AddressDto>();
        InvalidExistingAddresses = new List<AddressDto>();
        BillingNewAddress = new AddressDto();
    }


    [JsonProperty("existing_addresses")]
    public IList<AddressDto> ExistingAddresses { get; set; }

    [JsonProperty("invalid_existing_addresses")]
    public IList<AddressDto> InvalidExistingAddresses { get; set; }


    [JsonProperty("billing_new_address")]
    public AddressDto BillingNewAddress { get; set; }


    [JsonProperty("ship_to_same_address")]
    public bool ShipToSameAddress { get; set; }

    [JsonProperty("ship_to_same_address_allowed")]
    public bool ShipToSameAddressAllowed { get; set; }

    /// <summary>
    /// Used on one-page checkout page
    /// </summary>

    [JsonProperty("new_address_preselected")]
    public bool NewAddressPreselected { get; set; }


    [JsonProperty("eu_vat_enabled")]
    public bool EuVatEnabled { get; set; }

    [JsonProperty("eu_vat_enabled_for_guests")]
    public bool EuVatEnabledForGuests { get; set; }

    [NopResourceDisplayName("Checkout.VatNumber")]

    [JsonProperty("vat_number")]
    public string VatNumber { get; set; }
}
