using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "CustomerForumSubscriptions")]
public partial record CustomerForumSubscriptionsDto : BaseNopDto
{
    public CustomerForumSubscriptionsDto()
    {
        ForumSubscriptions = new List<ForumSubscriptionDto>();
    }


    [JsonProperty("forum_subscriptions")]
    public IList<ForumSubscriptionDto> ForumSubscriptions { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }


}
