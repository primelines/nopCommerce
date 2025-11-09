using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Catalog;

[JsonObject(Title = "TopMenu")]
public partial record TopMenuDto : BaseNopDto
{
    public TopMenuDto()
    {
        Categories = new List<CategorySimpleDto>();
        Topics = new List<MenuTopicDto>();
    }


    [JsonProperty("categories")]
    public IList<CategorySimpleDto> Categories { get; set; }

    [JsonProperty("topics")]
    public IList<MenuTopicDto> Topics { get; set; }


    [JsonProperty("blog_enabled")]
    public bool BlogEnabled { get; set; }

    [JsonProperty("new_products_enabled")]
    public bool NewProductsEnabled { get; set; }

    [JsonProperty("forum_enabled")]
    public bool ForumEnabled { get; set; }


    [JsonProperty("display_homepage_menu_item")]
    public bool DisplayHomepageMenuItem { get; set; }

    [JsonProperty("display_new_products_menu_item")]
    public bool DisplayNewProductsMenuItem { get; set; }

    [JsonProperty("display_product_search_menu_item")]
    public bool DisplayProductSearchMenuItem { get; set; }

    [JsonProperty("display_customer_info_menu_item")]
    public bool DisplayCustomerInfoMenuItem { get; set; }

    [JsonProperty("display_blog_menu_item")]
    public bool DisplayBlogMenuItem { get; set; }

    [JsonProperty("display_forums_menu_item")]
    public bool DisplayForumsMenuItem { get; set; }

    [JsonProperty("display_contact_us_menu_item")]
    public bool DisplayContactUsMenuItem { get; set; }


    [JsonProperty("use_ajax_menu")]
    public bool UseAjaxMenu { get; set; }

    public bool HasOnlyCategories => Categories.Any()
                                     && !Topics.Any()
                                     && !DisplayHomepageMenuItem
                                     && !(DisplayNewProductsMenuItem && NewProductsEnabled)
                                     && !DisplayProductSearchMenuItem
                                     && !DisplayCustomerInfoMenuItem
                                     && !(DisplayBlogMenuItem && BlogEnabled)
                                     && !(DisplayForumsMenuItem && ForumEnabled)
                                     && !DisplayContactUsMenuItem;

}
