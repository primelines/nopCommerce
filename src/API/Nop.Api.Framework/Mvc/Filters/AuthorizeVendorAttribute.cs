using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Data;
using Nop.Services.Security;

namespace Nop.Api.Framework.Mvc.Filters;

/// <summary>
/// Represents a filter attribute that confirms access to the vendor panel
/// </summary>
public sealed class AuthorizeVendorAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    /// <param name="ignore">Whether to ignore the execution of filter actions</param>
    public AuthorizeVendorAttribute(bool ignore = false) : base(typeof(AuthorizeVendorFilter))
    {
        IgnoreFilter = ignore;
        Arguments = [ignore];
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to ignore the execution of filter actions
    /// </summary>
    public bool IgnoreFilter { get; }

    #endregion

    #region Nested filter

    /// <summary>
    /// Represents a filter that confirms access to the vendor panel
    /// </summary>
    private class AuthorizeVendorFilter : IAsyncAuthorizationFilter
    {
        #region Fields

        protected readonly bool _ignoreFilter;
        protected readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public AuthorizeVendorFilter(bool ignoreFilter, IPermissionService permissionService)
        {
            _ignoreFilter = ignoreFilter;
            _permissionService = permissionService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized
        /// </summary>
        /// <param name="context">Authorization filter context</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task AuthorizeVendorAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //check whether this filter has been overridden for the action
            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<AuthorizeVendorAttribute>()
                .FirstOrDefault();

            //ignore filter (the action is available even if a customer hasn't access to the vendor area)
            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return;

            //there is VendorAuthorizeFilter, so check access
            if (context.Filters.Any(filter => filter is AuthorizeVendorFilter))
            {
                //authorize permission of access to the vendor area
                if (!await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL))
                    context.Result = new ChallengeResult();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized
        /// </summary>
        /// <param name="context">Authorization filter context</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await AuthorizeVendorAsync(context);
        }

        #endregion
    }

    #endregion
}