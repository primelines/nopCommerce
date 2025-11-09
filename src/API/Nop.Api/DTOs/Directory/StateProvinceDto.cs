using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Directory;

[JsonObject(Title = "StateProvince")]
public partial record StateProvinceDto : BaseNopDto
{

    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("name")]
    public string name { get; set; }
}
