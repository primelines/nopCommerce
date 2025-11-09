using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.PrivateMessages;

[JsonObject(Title = "PrivateMessageList")]
public partial record PrivateMessageListDto : BaseNopDto
{

    [JsonProperty("messages")]
    public IList<PrivateMessageDto> Messages { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }
}
