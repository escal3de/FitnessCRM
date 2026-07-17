using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Users;

public class UpdateUserHandler(
    IUsersRepository repository,
    IValidator<UpdateUserRequest> validator,
    ICacheService cache)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<UpdateUserRequest> _validator = validator;
    private readonly ICacheService _cache = cache;

    public async Task<Result> HandleAsync(Guid id, UpdateUserRequest request,
        CancellationToken cancellationToken, Guid? updaterId = null)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ",  validationResult.Errors.Select(x => x.ErrorMessage)));
        
        var userResult = await _repository.GetByIdAsync(id, cancellationToken);
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        if (updaterId.HasValue)
        {
            var updaterResult = await _repository.GetByIdAsync(updaterId.Value, cancellationToken);
            if (updaterResult.IsFailure)
                return Result.Failure(updaterResult.Error);

            if ((int)userResult.Value.Role > (int)updaterResult.Value.Role)
                return Result.Failure("You cannot update this user: their role is higher than yours");
        }

        var user = userResult.Value;

        Phone? phone = null;
        if (request.Phone is not null)
        {
            var phoneResult = Phone.Create(request.Phone);
            if (phoneResult.IsFailure)
                return Result.Failure(phoneResult.Error);

            phone = phoneResult.Value;
        }

        Email? email = null;
        if (request.Email is not null)
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
                return Result.Failure(emailResult.Error);

            email = emailResult.Value;
        }

        UserGender? gender = user.ContactData?.UserGender;

        if (gender is null)
        {
            if (request.Gender is null)
                return Result.Failure("Gender is required when user has no contact data");

            if (!Enum.TryParse<UserGender>(request.Gender, true, out var parsed))
                return Result.Failure($"Invalid gender: {request.Gender}");

            gender = parsed;
        }
        else if (request.Gender is not null)
        {
            if (!Enum.TryParse<UserGender>(request.Gender, true, out var parsed))
                return Result.Failure($"Invalid gender: {request.Gender}");

            gender = parsed;
        }

        var existing = user.ContactData;

        var contactData = ContactData.Load(
            request.AvatarUrl ?? existing?.AvatarUrl,
            email ?? existing?.Email,
            request.Name ?? existing?.Name ?? string.Empty,
            request.LastName ?? existing?.LastName ?? string.Empty,
            request.Patronymic ?? existing?.Patronymic ?? string.Empty,
            request.Birthday ?? existing?.Birthday,
            gender.Value,
            request.Profession ?? existing?.Profession,
            DateTime.UtcNow,
            existing?.CreatedAt ?? DateTime.UtcNow);

        var updatedUser = User.Load(
            user.Id,
            phone ?? user.Phone,
            contactData,
            user.ClientProfile,
            user.TeacherProfile,
            user.StaffProfile,
            user.Role,
            user.RefreshTokenHash,
            user.RefreshTokenExpiresAt,
            user.HashedPassword,
            user.TokenVersion,
            user.IsTwoFactorEnabled,
            DateTime.UtcNow,
            user.CreatedAt);

        var updateResult = await _repository.UpdateAsync(updatedUser, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult;

        await _cache.RemoveAsync($"cache:users:{id}", cancellationToken);

        return Result.Success();
    }
}
