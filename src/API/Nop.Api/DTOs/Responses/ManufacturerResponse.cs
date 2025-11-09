using Newtonsoft.Json;
using Nop.Api.DTOs.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ManufacturerResponse")]
public partial record ManufacturerResponse : BaseNopDto
{
    [JsonProperty("template_view_path")]
    public string TemplateViewPath { get; set; }

    [JsonProperty("manufacturer")]
    public ManufacturerDto Manufacturer { get; set; }

}
