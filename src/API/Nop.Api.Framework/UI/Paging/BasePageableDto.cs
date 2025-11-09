//Contributor : MVCContrib

using Nop.Core;
using Nop.Api.Framework.Dtos;
using Newtonsoft.Json;

namespace Nop.Api.Framework.UI.Paging;

/// <summary>
/// Base class for pageable models
/// </summary>
public abstract record BasePageableDto : BaseNopDto, IPageableDto
{
    #region Methods

    /// <summary>
    /// Ctor
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="pagedList">Entities (models)</param>
    public virtual void LoadPagedList<T>(IPagedList<T> pagedList)
    {
        FirstItem = (pagedList.PageIndex * pagedList.PageSize) + 1;
        HasNextPage = pagedList.HasNextPage;
        HasPreviousPage = pagedList.HasPreviousPage;
        LastItem = Math.Min(pagedList.TotalCount, ((pagedList.PageIndex * pagedList.PageSize) + pagedList.PageSize));
        PageNumber = pagedList.PageIndex + 1;
        PageSize = pagedList.PageSize;
        TotalItems = pagedList.TotalCount;
        TotalPages = pagedList.TotalPages;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The current page index (starts from 0)
    /// </summary>
    [JsonProperty("page_index")]
    public int PageIndex
    {
        get
        {
            if (PageNumber > 0)
                return PageNumber - 1;

            return 0;
        }
    }

    /// <summary>
    /// The current page number (starts from 1)
    /// </summary>
    [JsonProperty("page_number")]
    public int PageNumber { get; set; }

    /// <summary>
    /// The number of items in each page.
    /// </summary>
    [JsonProperty("page_size")]
    public int PageSize { get; set; } = 5;

    /// <summary>
    /// The total number of items.
    /// </summary>
    [JsonProperty("total_items")]
    public int TotalItems { get; set; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// The index of the first item in the page.
    /// </summary>
    [JsonProperty("first_item")]
    public int FirstItem { get; set; }

    /// <summary>
    /// The index of the last item in the page.
    /// </summary>
    [JsonProperty("last_item")]
    public int LastItem { get; set; }

    /// <summary>
    /// Whether there are pages before the current page.
    /// </summary>
    [JsonProperty("has_previous_page")]
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Whether there are pages after the current page.
    /// </summary>
    [JsonProperty("has_next_page")]
    public bool HasNextPage { get; set; }

    #endregion
}