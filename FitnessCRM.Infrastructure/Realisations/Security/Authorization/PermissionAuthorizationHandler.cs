using FitnessCRM.Infrastructure.Realisations.Security.Authorization.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCRM.Infrastructure.Realisations.Security.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            context.Succeed(requirement);
        else
            context.Fail();
        
        return Task.CompletedTask;
    }
}