using Microsoft.AspNetCore.Authorization;

namespace FitnessCRM.Infrastructure.Realisations.Security.Authorization.Requirement;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}