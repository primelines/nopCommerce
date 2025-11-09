using Nop.Core.Domain.News;
using Nop.Api.DTOs.News;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the interface of the news model factory
/// </summary>
public partial interface INewsDtoFactory
{
    /// <summary>
    /// Prepare the news item model
    /// </summary>
    /// <param name="model">News item model</param>
    /// <param name="newsItem">News item</param>
    /// <param name="prepareComments">Whether to prepare news comment models</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the news item model
    /// </returns>
    Task<NewsItemDto> PrepareNewsItemDtoAsync(NewsItemDto model, NewsItem newsItem, bool prepareComments);

    /// <summary>
    /// Prepare the home page news items model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the home page news items model
    /// </returns>
    Task<HomepageNewsItemsDto> PrepareHomepageNewsItemsModelAsync();

    /// <summary>
    /// Prepare the news item list model
    /// </summary>
    /// <param name="command">News paging filtering model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the news item list model
    /// </returns>
    Task<NewsItemListDto> PrepareNewsItemListDtoAsync(NewsPagingFilteringDto command);

    /// <summary>
    /// Prepare the news comment model
    /// </summary>
    /// <param name="newsComment">News comment</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the news comment model
    /// </returns>
    Task<NewsCommentDto> PrepareNewsCommentDtoAsync(NewsComment newsComment);
}