using System.Security.Cryptography;
using System.Text;
using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Application.Mappers;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Handlers.Auth.Refresh;

public class RefreshTokenHandler(
    IUsersRepository repository,
    IJwtProvider jwtProvider)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<LoginUserResponse>> HandleAsync(string rawRefreshToken,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByRefreshTokenAsync(rawRefreshToken, cancellationToken);
        if (user.IsFailure)
            return Result.Failure<LoginUserResponse>(user.Error);

        if (user.Value.RefreshTokenExpiresAt < DateTime.UtcNow)
            return Result.Failure<LoginUserResponse>("Refresh token expired");

        if (user.Value.Role == UserRole.Banned)
            return Result.Failure<LoginUserResponse>("Account is banned");

        var newAccessToken = _jwtProvider.GenerateJwtToken(user.Value);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();
        var newRefreshTokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(newRefreshToken)));
        var nexExpires = DateTime.UtcNow.AddDays(7);

        await _repository.UpdateRefreshTokenAsync(user.Value.Id, newRefreshTokenHash, nexExpires, cancellationToken);

        return Result.Success(user.Value.Map(newAccessToken, newRefreshToken));
    }
}