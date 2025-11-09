using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "CurrencySelector")]
public partial record CurrencySelectorDto : BaseNopDto
{
    public CurrencySelectorDto()
    {
        AvailableCurrencies = new List<CurrencyDto>();
    }


    [JsonProperty("available_currencies")]
    public IList<CurrencyDto> AvailableCurrencies { get; set; }


    [JsonProperty("current_currency_id")]
    public int CurrentCurrencyId { get; set; }
}
