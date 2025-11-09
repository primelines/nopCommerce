using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "Vendor")]
public partial record VendorDto : BaseNopEntityDto
{
    public VendorDto()
    {
        ProfilePicture = new PictureDto();
        CatalogProducts = new CatalogProductsDto();
    }


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("bio")]
    public string Bio { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }

    [JsonProperty("allow_customers_to_contact_vendors")]
    public bool AllowCustomersToContactVendors { get; set; }


    [JsonProperty("profile_picture")]
    public PictureDto ProfilePicture { get; set; }


    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }
}
