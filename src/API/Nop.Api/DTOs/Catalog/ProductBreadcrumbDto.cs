using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "ProductBreadcrumb")]
public partial record ProductBreadcrumbDto : BaseNopDto
    {
        public ProductBreadcrumbDto()
        {
            CategoryBreadcrumb = new List<CategorySimpleDto>();
        }


    [JsonProperty("enabled")]
        public bool Enabled { get; set; }

    [JsonProperty("json_ld")]
        public string JsonLd { get; set; }

    [JsonProperty("product_id")]
        public int ProductId { get; set; }

    [JsonProperty("product_name")]
        public string ProductName { get; set; }

    [JsonProperty("product_se_name")]
        public string ProductSeName { get; set; }

    [JsonProperty("category_breadcrumb")]
        public IList<CategorySimpleDto> CategoryBreadcrumb { get; set; }
    }


