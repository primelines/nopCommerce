using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.PrivateMessages;

[JsonObject(Title = "PrivateMessageIndex")]
public partial record PrivateMessageIndexDto : BaseNopDto
{

    [JsonProperty("inbox_page")]
    public int InboxPage { get; set; }

    [JsonProperty("sent_items_page")]
    public int SentItemsPage { get; set; }

    [JsonProperty("sent_items_tab_selected")]
    public bool SentItemsTabSelected { get; set; }
}
