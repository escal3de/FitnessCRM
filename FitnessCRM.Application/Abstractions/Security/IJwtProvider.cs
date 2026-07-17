using FitnessCRM.Domain;

namespace FitnessCRM.Application.Abstractions.Security;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
}