using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogPostYear")]
public partial record BlogPostYearDto : BaseNopDto
{
    public BlogPostYearDto()
    {
        Months = new List<BlogPostMonthModel>();
    }

    [JsonProperty("year")]
    public int Year { get; set; }

    [JsonProperty("months")]
    public IList<BlogPostMonthModel> Months { get; set; }
}

[JsonObject(Title = "BlogPostMonth")]
public partial record BlogPostMonthModel : BaseNopDto
{

    [JsonProperty("month")]
    public int Month { get; set; }


    [JsonProperty("blog_post_count")]
    public int BlogPostCount { get; set; }
}
