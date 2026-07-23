using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using Serilog;

namespace FitnessCRM.Api.Extensions;

public static class ClaimsExtension
{
    public static Result<Guid> TryGetUserGuid(this ClaimsPrincipal user)
    {
        var guid = user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (guid is null)
        {
            Log.Warning("Guid not found in claims");
            return Result.Failure<Guid>("Guid not found in claims");
        }

        if (!Guid.TryParse(guid, out var result))
        {
            Log.Warning("Guid not parsed");
            return Result.Failure<Guid>("Guid not parsed");
        }
        
        return Result.Success(result);
    }
    
    public static Result<Guid> TryGetUserSchool(this ClaimsPrincipal user)
    {
        var guid = user.Claims.FirstOrDefault(x => x.Type == "schoolId")?.Value;

        if (guid is null)
        {
            Log.Warning("School id not found in claims");
            return Result.Failure<Guid>("School id not found in claims");
        }

        if (!Guid.TryParse(guid, out var result))
        {
            Log.Warning("School id not parsed");
            return Result.Failure<Guid>("School id not parsed");
        }
        
        return Result.Success(result);
    }
}