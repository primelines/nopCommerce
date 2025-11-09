using Newtonsoft.Json;
using Nop.Core.Domain.Tax;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "TaxTypeSelector")]
public partial record TaxTypeSelectorDto : BaseNopDto
{

    [JsonProperty("current_tax_type")]
    public TaxDisplayType CurrentTaxType { get; set; }
}
