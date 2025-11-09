using Newtonsoft.Json;
using Newtonsoft.Json;
using Nop.Api.DTOs.Catalog;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Responses;

[JsonObject(Title = "ProductDetailsResponse")]
public partial record ProductDetailsResponse : BaseNopDto
{

    /// <summary>
    /// The product template view path
    /// </summary>
    [JsonProperty("product_template_view_path")]
    public string ProductTemplateViewPath { get; set; }

    [JsonProperty("product_details")]
    public ProductDetailsDto ProductDetails { get; set; }


}
