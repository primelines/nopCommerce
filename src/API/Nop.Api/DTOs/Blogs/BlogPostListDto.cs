using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogPostList")]
public partial record BlogPostListDto : BaseNopDto
{
    public BlogPostListDto()
    {
        PagingFilteringContext = new BlogPagingFilteringDto();
        BlogPosts = new List<BlogPostDto>();
    }


    [JsonProperty("working_language_id")]
    public int WorkingLanguageId { get; set; }

    [JsonProperty("paging_filtering_context")]
    public BlogPagingFilteringDto PagingFilteringContext { get; set; }

    [JsonProperty("blog_posts")]
    public IList<BlogPostDto> BlogPosts { get; set; }
}
