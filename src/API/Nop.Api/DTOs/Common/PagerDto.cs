using Newtonsoft.Json;
using Nop.Services.Localization;
using Nop.Api.Infrastructure;

namespace Nop.Api.DTOs.Common;

[JsonObject(Title = "Pager")]
public partial record PagerDto
{
    #region Fields

    protected readonly ILocalizationService _localizationService;

    protected int _individualPagesDisplayedCount;
    protected int _pageIndex = -2;
    protected int _pageSize;

    protected bool? _showFirst;
    protected bool? _showIndividualPages;
    protected bool? _showLast;
    protected bool? _showNext;
    protected bool? _showPagerItems;
    protected bool? _showPrevious;
    protected bool? _showTotalSummary;

    #endregion Fields

    #region Ctor

    public PagerDto(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the first button text
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<string> GetFirstButtonTextAsync()
    {
        return await _localizationService.GetResourceAsync("Pager.First");
    }

    /// <summary>
    /// Gets the last button text
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<string> GetLastButtonTextAsync()
    {
        return await _localizationService.GetResourceAsync("Pager.Last");
    }

    /// <summary>
    /// Gets the next button text
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<string> GetNextButtonTextAsync()
    {
        return await _localizationService.GetResourceAsync("Pager.Next");
    }

    /// <summary>
    /// Gets the previous button text
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<string> GetPreviousButtonTextAsync()
    {
        return await _localizationService.GetResourceAsync("Pager.Previous");
    }

    /// <summary>
    /// Gets or sets the current page text
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<string> GetCurrentPageTextAsync()
    {
        return await _localizationService.GetResourceAsync("Pager.CurrentPage");
    }

    /// <summary>
    /// Gets first individual page index
    /// </summary>
    /// <returns>Page index</returns>
    public int GetFirstIndividualPageIndex()
    {
        if ((TotalPages < IndividualPagesDisplayedCount) ||
            ((PageIndex - (IndividualPagesDisplayedCount / 2)) < 0))
            return 0;

        if ((PageIndex + (IndividualPagesDisplayedCount / 2)) >= TotalPages) 
            return (TotalPages - IndividualPagesDisplayedCount);

        return (PageIndex - (IndividualPagesDisplayedCount / 2));
    }

    /// <summary>
    /// Get last individual page index
    /// </summary>
    /// <returns>Page index</returns>
    public int GetLastIndividualPageIndex()
    {
        var num = IndividualPagesDisplayedCount / 2;
        if ((IndividualPagesDisplayedCount % 2) == 0) 
            num--;

        if ((TotalPages < IndividualPagesDisplayedCount) ||
            ((PageIndex + num) >= TotalPages))
            return (TotalPages - 1);

        if ((PageIndex - (IndividualPagesDisplayedCount / 2)) < 0) 
            return (IndividualPagesDisplayedCount - 1);

        return PageIndex + num;
    }

    #endregion Methods

    #region Properties

    /// <summary>
    /// Gets the current page index
    /// </summary>
    [JsonProperty("current_page")]
    public int CurrentPage => PageIndex + 1;

    /// <summary>
    /// Gets or sets a count of individual pages to be displayed
    /// </summary>
    [JsonProperty("individual_pages_displayed_count")]
    public int IndividualPagesDisplayedCount
    {
        get
        {
            if (_individualPagesDisplayedCount <= 0)
                return 5;

            return _individualPagesDisplayedCount;
        }
        set => _individualPagesDisplayedCount = value;
    }

    /// <summary>
    /// Gets the current page index
    /// </summary>
    [JsonProperty("page_index")]
    public int PageIndex
    {
        get => _pageIndex < 0 ? 0 : _pageIndex;
        set => _pageIndex = value;
    }

    /// <summary>
    /// Gets or sets a page size
    /// </summary>
    [JsonProperty("page_size")]
    public int PageSize
    {
        get => (_pageSize <= 0) ? 10 : _pageSize;
        set => _pageSize = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "first"
    /// </summary>
    [JsonProperty("show_first")]
    public bool ShowFirst
    {
        get => _showFirst ?? true;
        set => _showFirst = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "individual pages"
    /// </summary>
    [JsonProperty("show_individual_pages")]
    public bool ShowIndividualPages
    {
        get => _showIndividualPages ?? true;
        set => _showIndividualPages = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "last"
    /// </summary>
    [JsonProperty("show_last")]
    public bool ShowLast
    {
        get => _showLast ?? true;
        set => _showLast = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "next"
    /// </summary>
    [JsonProperty("show_next")]
    public bool ShowNext
    {
        get => _showNext ?? true;
        set => _showNext = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show pager items
    /// </summary>
    [JsonProperty("show_pager_items")]
    public bool ShowPagerItems
    {
        get => _showPagerItems ?? true;
        set => _showPagerItems = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "previous"
    /// </summary>
    [JsonProperty("show_previous")]
    public bool ShowPrevious
    {
        get => _showPrevious ?? true;
        set => _showPrevious = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show "total summary"
    /// </summary>
    [JsonProperty("show_total_summary")]
    public bool ShowTotalSummary
    {
        get => _showTotalSummary ?? false;
        set => _showTotalSummary = value;
    }

    /// <summary>
    /// Gets a total pages count
    /// </summary>
    [JsonProperty("total_pages")]
    public int TotalPages
    {
        get
        {
            if (TotalRecords == 0 || PageSize == 0)
                return 0;

            var num = TotalRecords / PageSize;

            if ((TotalRecords % PageSize) > 0)
                num++;

            return num;
        }
    }

    /// <summary>
    /// Gets or sets a total records count
    /// </summary>

    [JsonProperty("total_records")]
    public int TotalRecords { get; set; }


    /// <summary>
    /// Gets or sets the route name or action name
    /// </summary>

    [JsonProperty("route_action_name")]
    public string RouteActionName { get; set; }

    /// <summary>
    /// Gets or sets whether the links are created using RouteLink instead of Action Link 
    /// (for additional route values such as slugs or page numbers)
    /// </summary>

    [JsonProperty("use_route_links")]
    public bool UseRouteLinks { get; set; }

    /// <summary>
    /// Gets or sets the RouteValues object. Allows for custom route values other than page.
    /// </summary>

    [JsonProperty("route_values")]
    public IRouteValues RouteValues { get; set; }

    #endregion Properties
}
