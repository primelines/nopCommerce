using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

public partial record MenuTopicDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }
}

