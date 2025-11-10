using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Customer;

[JsonObject(Title = "CustomerNavigation")]
public partial record CustomerNavigationDto : BaseNopDto
{
    public CustomerNavigationDto()
    {
        CustomerNavigationItems = new List<CustomerNavigationItemModel>();
    }


    [JsonProperty("customer_navigation_items")]
    public IList<CustomerNavigationItemModel> CustomerNavigationItems { get; set; }


    [JsonProperty("selected_tab")]
    public int SelectedTab { get; set; }
}

[JsonObject(Title = "CustomerNavigationItem")]
public partial record CustomerNavigationItemModel : BaseNopDto
{

    [JsonProperty("route_name")]
    public string RouteName { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("tab")]
    public int Tab { get; set; }

    [JsonProperty("item_class")]
    public string ItemClass { get; set; }
}

public enum CustomerNavigationEnum
{
    Info = 0,
    Addresses = 10,
    Orders = 20,
    BackInStockSubscriptions = 30,
    ReturnRequests = 40,
    RewardPoints = 60,
    ChangePassword = 70,
    Avatar = 80,
    ForumSubscriptions = 90,
    ProductReviews = 100,
    VendorInfo = 110,
    GdprTools = 120,
    CheckGiftCardBalance = 130,
    MultiFactorAuthentication = 140
}
