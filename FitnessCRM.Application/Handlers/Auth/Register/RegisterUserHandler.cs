using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain;
using FitnessCRM.Domain.VO;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Auth.Register;

public class RegisterUserHandler(
    IUsersRepository repository,
    IValidator<RegisterUserRequest> validator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<RegisterUserRequest> _validator = validator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var phone = Phone.Create(request.Phone);
        if (phone.IsFailure)
            return Result.Failure<Phone>(phone.Error);

        var existsUser = await _repository.GetByPhoneAsync(phone.Value, cancellationToken);
        if (existsUser.IsSuccess)
            return Result.Failure("User with this phone already exists");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(phone.Value, passwordHash);
        if (user.IsFailure)
            return Result.Failure(user.Error);
        
        await _repository.AddAsync(user.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}