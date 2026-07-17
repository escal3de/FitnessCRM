using System.Security.Cryptography;
using System.Text;
using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Application.Mappers;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Auth.Login;

public class LoginUserHandler(
    IUsersRepository repository,
    IValidator<LoginUserRequest> validator,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<LoginUserRequest> _validator = validator;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LoginUserResponse>> HandleAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure<LoginUserResponse>(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var phone = Phone.Create(request.Phone);
        if (phone.IsFailure)
            return Result.Failure<LoginUserResponse>(phone.Error);
        
        var user = await _repository.GetByPhoneAsync(phone.Value, cancellationToken);
        if (user.IsFailure)
            return Result.Failure<LoginUserResponse>("Invalid credentials");

        var checkPassword = _passwordHasher.VerifyHashedPassword(user.Value.HashedPassword, request.Password);
        if (!checkPassword)
            return Result.Failure<LoginUserResponse>("Invalid credentials");

        if (user.Value.Role == UserRole.Banned)
            return Result.Failure<LoginUserResponse>("Account is banned");

        var token = _jwtProvider.GenerateJwtToken(user.Value);
        var refreshToken = _jwtProvider.GenerateRefreshToken();
        var refreshTokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7);

        await _repository.UpdateRefreshTokenAsync(user.Value.Id, refreshTokenHash, refreshTokenExpirationDate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Value.Map(token, refreshToken));
    }
}