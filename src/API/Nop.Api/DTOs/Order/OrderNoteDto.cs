using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Order;
[JsonObject(Title = "OrderNote")]
public partial record OrderNoteDto : BaseNopEntityDto
{

    [JsonProperty("has_download")]
    public bool HasDownload { get; set; }

    [JsonProperty("note")]
    public string Note { get; set; }

    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }
}
