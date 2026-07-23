using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Domain.Enums.Users;
using Microsoft.AspNetCore.Http;

namespace FitnessCRM.Infrastructure.Realisations.Isolating;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var value = User?.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return value is not null && Guid.TryParse(value, out var result) ? result : Guid.Empty;
        }
    }

    public Guid? SchoolId
    {
        get
        {
            var value = User?.Claims.FirstOrDefault(x => x.Type == "schoolId")?.Value;
            return value is not null && Guid.TryParse(value, out var result) ? result : null;
        }
    }

    public UserRole UserRole
    {
        get
        {
            var value = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return value is not null && Enum.TryParse<UserRole>(value, out var result) ? result : UserRole.Default;
        }
    }
}
