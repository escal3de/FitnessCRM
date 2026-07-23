using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Users;

public class CreateUserHandler(
    IUsersRepository repository,
    IValidator<CreateUserRequest> validator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    ICurrentUserService current)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<CreateUserRequest> _validator = validator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _current = current;

    public async Task<Result> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var phone = Phone.Create(request.Phone);
        if (phone.IsFailure)
            return Result.Failure(phone.Error);

        var exists = await _repository.GetByPhoneAsync(phone.Value, cancellationToken);
        if (exists.IsSuccess)
            return Result.Failure("A user with such a phone already exists");

        if (!(request.Role is UserRole.Default or UserRole.Teacher or UserRole.Staff))
            return Result.Failure("You cannot create a user with this role");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(phone.Value, passwordHash);
        if (user.IsFailure)
            return Result.Failure(user.Error);

        if (_current.SchoolId.HasValue)
        {
            var schoolResult = await _repository.GetSchoolByIdAsync(_current.SchoolId.Value, cancellationToken);
            if (schoolResult.IsSuccess)
                user.Value.BindToSchool(schoolResult.Value);
        }

        var profile = AddUserProfile(user.Value, request.Role);
        if (profile.IsFailure)
            return Result.Failure(profile.Error);

        await _repository.AddAsync(user.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result AddUserProfile(User user, UserRole role)
    {
        switch (role)
        {
            case UserRole.Default:
                var clientResult = ClientProfile.Create();
                if (clientResult.IsFailure)
                    return Result.Failure(clientResult.Error);
                user.AddClientProfile(clientResult.Value);
                break;

            case UserRole.Teacher:
                var teacherResult = TeacherProfile.Create();
                if (teacherResult.IsFailure)
                    return Result.Failure(teacherResult.Error);
                user.AddTeacherProfile(teacherResult.Value);
                break;

            case UserRole.Staff:
                var staffResult = StaffProfile.Create();
                if (staffResult.IsFailure)
                    return Result.Failure(staffResult.Error);
                user.AddStaffProfile(staffResult.Value);
                break;
        }

        return Result.Success();
    } 
}
