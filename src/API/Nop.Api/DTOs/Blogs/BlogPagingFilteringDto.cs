using Newtonsoft.Json;
using Nop.Api.Framework.UI.Paging;

namespace Nop.Api.DTOs.Blogs;

[JsonObject(Title = "BlogPagingFiltering")]
public partial record BlogPagingFilteringDto : BasePageableDto
{
    #region Methods

    public virtual DateTime? GetParsedMonth()
    {
        DateTime? result = null;
        if (!string.IsNullOrEmpty(Month))
        {
            var tempDate = Month.Split(['-']);
            if (tempDate.Length == 2)
            {
                result = new DateTime(Convert.ToInt32(tempDate[0]), Convert.ToInt32(tempDate[1]), 1);
            }
        }
        return result;
    }
    public virtual DateTime? GetFromMonth()
    {
        var filterByMonth = GetParsedMonth();
        if (filterByMonth.HasValue)
            return filterByMonth.Value;
        return null;
    }
    public virtual DateTime? GetToMonth()
    {
        var filterByMonth = GetParsedMonth();
        if (filterByMonth.HasValue)
            return filterByMonth.Value.AddMonths(1).AddSeconds(-1);
        return null;
    }
    #endregion

    #region Properties


    [JsonProperty("month")]
    public string Month { get; set; }


    [JsonProperty("tag")]
    public string Tag { get; set; }

    #endregion
}
