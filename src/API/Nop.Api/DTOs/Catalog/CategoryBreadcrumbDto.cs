using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CategoryBreadcrumb")]
public partial record CategoryBreadcrumbDto : BaseNopEntityDto
{

    [JsonProperty("breadcrumb")]
    public string Breadcrumb { get; set; }
}
