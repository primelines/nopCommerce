using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductTag")]
public partial record ProductTagDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("product_count")]
    public int ProductCount { get; set; }
}
