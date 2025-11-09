using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Nop.Api.Infrastructure.Swagger
{
    /// <summary>
    /// Rewrites the operationId to be based on method name + HTTP verb
    /// </summary>
    public sealed class OperationIdRewriteFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null || context.MethodInfo == null)
                return;

            // Get the actual method name (e.g., "SubscribePopup")
            var methodName = context.MethodInfo.Name;

            // Get HTTP verb (e.g., GET / POST)
            var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant() ?? "GET";


            // If the action is Index, use ControllerName_Index
            if (methodName == "Index")
            {
                var controllerName = context.ApiDescription.ActionDescriptor.RouteValues["controller"];
                if (!string.IsNullOrWhiteSpace(controllerName))
                {
                    operation.OperationId = $"{controllerName}_Index_{httpMethod}";
                    return;
                }
            }


            // Rewrite operationId
            operation.OperationId = $"{methodName}_{httpMethod}";
        }
    }
}
