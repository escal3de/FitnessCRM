using FitnessCRM.Infrastructure.Realisations.Security.Authorization.Requirement;

namespace FitnessCRM.Api.Extensions;

public static class AuthorizationExtension
{
    public static RouteGroupBuilder RequirePermission(this RouteGroupBuilder builder, string permission)
        => builder.RequireAuthorization(pb => pb.AddRequirements(new PermissionRequirement(permission)));

    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string permission)
        => builder.RequireAuthorization(pb => pb.AddRequirements(new PermissionRequirement(permission)));
}