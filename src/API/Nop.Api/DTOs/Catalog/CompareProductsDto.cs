using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "CompareProducts")]
public partial record CompareProductsDto : BaseNopEntityDto
{
    public CompareProductsDto()
    {
        Products = new List<ProductOverviewDto>();
    }

    [JsonProperty("products")]
    public IList<ProductOverviewDto> Products { get; set; }


    [JsonProperty("include_short_description_in_compare_products")]
    public bool IncludeShortDescriptionInCompareProducts { get; set; }

    [JsonProperty("include_full_description_in_compare_products")]
    public bool IncludeFullDescriptionInCompareProducts { get; set; }
}
