using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "Category")]
public partial record CategoryDto : BaseNopEntityDto
{
    public CategoryDto()
    {
        Picture = new PictureDto();
        FeaturedProducts = new List<ProductOverviewDto>();
        SubCategories = new List<SubCategoryDto>();
        CategoryBreadcrumb = new List<CategoryDto>();
        CatalogProducts = new CatalogProductsDto();
    }


    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("meta_keywords")]
    public string MetaKeywords { get; set; }

    [JsonProperty("meta_description")]
    public string MetaDescription { get; set; }

    [JsonProperty("meta_title")]
    public string MetaTitle { get; set; }

    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("picture")]
    public PictureDto Picture { get; set; }


    [JsonProperty("display_category_breadcrumb")]
    public bool DisplayCategoryBreadcrumb { get; set; }

    [JsonProperty("category_breadcrumb")]
    public IList<CategoryDto> CategoryBreadcrumb { get; set; }


    [JsonProperty("sub_categories")]
    public IList<SubCategoryDto> SubCategories { get; set; }


    [JsonProperty("featured_products")]
    public IList<ProductOverviewDto> FeaturedProducts { get; set; }


    [JsonProperty("catalog_products")]
    public CatalogProductsDto CatalogProducts { get; set; }


    [JsonProperty("json_ld")]
    public string JsonLd { get; set; }

    #region Nested Classes

    public partial record SubCategoryDto : BaseNopEntityDto
    {
        public SubCategoryDto()
        {
            Picture = new PictureDto();
        }


    [JsonProperty("name")]
        public string Name { get; set; }


    [JsonProperty("se_name")]
        public string SeName { get; set; }


    [JsonProperty("description")]
        public string Description { get; set; }


    [JsonProperty("picture")]
        public PictureDto Picture { get; set; }
    }

    #endregion
}
