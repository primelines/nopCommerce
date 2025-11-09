using Newtonsoft.Json;
using Nop.Api.DTOs.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "CategoryResponse")]
public partial record CategoryResponse : BaseNopDto
{
    [JsonProperty("template_view_path")]
    public string TemplateViewPath { get; set; }

    [JsonProperty("category")]
    public CategoryDto Category { get; set; }
}
