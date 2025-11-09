using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Api.Framework.Events;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.Framework.Mvc.Filters;

/// <summary>
/// Represents filter attribute that publish ModelReceived event before the action executes, after model binding is complete
/// and publish ModelPrepared event after the action executes, before the action result
/// </summary>
public sealed class PublishDtoEventsAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    /// <param name="ignore">Whether to ignore the execution of filter actions</param>
    public PublishDtoEventsAttribute(bool ignore = false) : base(typeof(PublishModelEventsFilter))
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
    /// Represents filter that publish ModelReceived event before the action executes, after model binding is complete
    /// and publish ModelPrepared event after the action executes, before the action result
    /// </summary>
    private class PublishModelEventsFilter : IAsyncActionFilter, IAsyncResultFilter
    {
        #region Fields

        protected readonly bool _ignoreFilter;
        protected readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public PublishModelEventsFilter(bool ignoreFilter,
            IEventPublisher eventPublisher)
        {
            _ignoreFilter = ignoreFilter;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Whether to ignore this filter
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <returns>Result</returns>
        protected virtual bool IgnoreFilter(FilterContext context)
        {
            //check whether this filter has been overridden for the Action
            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<PublishDtoEventsAttribute>()
                .FirstOrDefault();

            return actionFilter?.IgnoreFilter ?? _ignoreFilter;
        }

        /// <summary>
        /// Publish model prepared event
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task PublishDtoPreparedEventAsync(object model)
        {
            //we publish the ModelPrepared event for all models as the BaseNopDto, 
            //so you need to implement IConsumer<ModelPrepared<BaseNopDto>> interface to handle this event
            if (model is BaseNopDto nopModel)
                await _eventPublisher.ModelPreparedAsync(nopModel);

            //we publish the ModelPrepared event for collection as the IEnumerable<BaseNopDto>, 
            //so you need to implement IConsumer<ModelPrepared<IEnumerable<BaseNopDto>>> interface to handle this event
            if (model is IEnumerable<BaseNopDto> nopModelCollection)
                await _eventPublisher.ModelPreparedAsync(nopModelCollection);
        }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task PublishDtoReceivedEventAsync(ActionExecutingContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            //only in POST requests
            if (!context.HttpContext.Request.IsPostRequest())
                return;

            if (IgnoreFilter(context))
                return;

            //model received event
            foreach (var model in context.ActionArguments.Values.OfType<BaseNopDto>())
            {
                //we publish the ModelReceived event for all models as the BaseNopDto, 
                //so you need to implement IConsumer<ModelReceived<BaseNopDto>> interface to handle this event
                await _eventPublisher.ModelReceivedAsync(model, context.ModelState);
            }
        }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task PublishDtoPreparedEventAsync(ActionExecutingContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (IgnoreFilter(context))
                return;

            //model prepared event
            if (context.Controller is Controller controller)
                await PublishDtoPreparedEventAsync(controller.ViewData.Model);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await PublishDtoReceivedEventAsync(context);
            if (context.Result == null)
                await next();
            await PublishDtoPreparedEventAsync(context);
        }

        /// <summary>Called asynchronously before the action result.</summary>
        /// <param name="context">A context for action filters</param>
        /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (IgnoreFilter(context))
                return;

            //model prepared event
            if (context.Result is JsonResult result)
                await PublishDtoPreparedEventAsync(result.Value);

            await next();
        }

        #endregion
    }

    #endregion
}