using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "GetTagProductsResponse")]
public partial record GetTagProductsResponse : BaseNopDto
{
    [JsonProperty("template_view_path")]
    public string TemplateViewPath { get; set; }

    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }
}
