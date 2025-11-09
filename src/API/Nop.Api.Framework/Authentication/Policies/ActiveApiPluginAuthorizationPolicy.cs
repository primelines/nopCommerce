using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nop.Api.Framework.Authorization.Requirements;

namespace Nop.Api.Framework.Authorization.Policies
{
    public class ActiveApiPluginAuthorizationPolicy : AuthorizationHandler<ActiveApiPluginRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveApiPluginRequirement requirement)
        {
            if (requirement.IsActive())
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
