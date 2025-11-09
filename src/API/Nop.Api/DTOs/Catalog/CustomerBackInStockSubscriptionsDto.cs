using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CustomerBackInStockSubscriptions")]
public partial record CustomerBackInStockSubscriptionsDto : BaseNopDto
{
    public CustomerBackInStockSubscriptionsDto()
    {
        Subscriptions = new List<BackInStockSubscriptionDto>();
    }


    [JsonProperty("subscriptions")]
    public IList<BackInStockSubscriptionDto> Subscriptions { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }

}
