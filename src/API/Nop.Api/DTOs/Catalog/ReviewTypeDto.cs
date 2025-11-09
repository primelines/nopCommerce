using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ReviewType")]
public partial record ReviewTypeDto : BaseNopEntityDto
{

    [JsonProperty("name")]
    public string Name { get; set; }


    [JsonProperty("description")]
    public string Description { get; set; }


    [JsonProperty("display_order")]
    public int DisplayOrder { get; set; }


    [JsonProperty("is_required")]
    public bool IsRequired { get; set; }


    [JsonProperty("visible_to_all_customers")]
    public bool VisibleToAllCustomers { get; set; }


    [JsonProperty("average_rating")]
    public double AverageRating { get; set; }
}
