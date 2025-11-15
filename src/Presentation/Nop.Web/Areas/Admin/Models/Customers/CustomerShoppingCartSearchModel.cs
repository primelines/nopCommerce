using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer shopping cart search model
/// </summary>
public partial record CustomerShoppingCartSearchModel : BaseSearchModel
{

    #region Properties

    public int CustomerId { get; set; }

    #endregion
}