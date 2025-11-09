using Newtonsoft.Json;
using Nop.Api.DTOs.ShoppingCart;
using Nop.Api.Framework.Dtos;
using System.Collections.Generic;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CheckoutAttributeChangeResponse")]
public partial record CheckoutAttributeChangeResponse : BaseNopDto
{
    [JsonProperty("order_totals")]
    public OrderTotalsDto OrderTotals { get; set; }

    [JsonProperty("formatted_attributes")]
    public string FormattedAttributes { get; set; }

    [JsonProperty("enabled_attribute_ids")]
    public IList<int> EnabledAttributeIds { get; set; }

    [JsonProperty("disabled_attribute_ids")]
    public IList<int> DisabledAttributeIds { get; set; }


}
