using Newtonsoft.Json;
namespace Nop.Api.DTOs.Sitemap;

/// <summary>
/// Represents an event that occurs when the sitemap is created
/// </summary>
[JsonObject(Title = "SitemapCreatedEvent")]
public partial class SitemapCreatedEvent
{
    #region Ctor

    public SitemapCreatedEvent(IList<SitemapUrlDto> sitemapUrls)
    {
        SitemapUrls = sitemapUrls ?? new List<SitemapUrlDto>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a list of sitemap URLs
    /// </summary>
    public IList<SitemapUrlDto> SitemapUrls { get; }

    #endregion
}
