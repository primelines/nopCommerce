using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a stock quantity history search model
/// </summary>
public partial record StockQuantityHistorySearchModel : BaseSearchModel
{
    #region Ctor

    public StockQuantityHistorySearchModel()
    {
    }

    #endregion

    #region Properties

    public int ProductId { get; set; }


    #endregion
}