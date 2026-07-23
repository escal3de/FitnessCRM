using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Permissions;
using FitnessCRM.Infrastructure.Realisations.Security.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FitnessCRM.Infrastructure.Realisations.Security;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string GenerateJwtToken(User user)
    {
        var permissions = MapPermissions.GetPermissionsForRole(user.Role);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("tv", user.TokenVersion.ToString()),
                ..permissions.Select(p => new Claim("permission", p))
        ];

        if (user.School is not null)
            claims.Add(new Claim("schoolId", user.School.Id.ToString()));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}