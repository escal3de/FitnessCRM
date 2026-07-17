using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Application.Handlers.Users;

public class UpdateUserHandler(
    IUsersRepository repository,
    ICacheService cache)
{
    private readonly IUsersRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    public async Task<Result> HandleAsync(Guid id, Guid updaterId, UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var userResult = await _repository.GetByIdAsync(id, cancellationToken);
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        var updaterResult = await _repository.GetByIdAsync(updaterId, cancellationToken);
        if (updaterResult.IsFailure)
            return Result.Failure(updaterResult.Error);

        if ((int)userResult.Value.Role > (int)updaterResult.Value.Role)
            return Result.Failure("You cannot update this user: their role is higher than yours");

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

        var updateResult = await _repository.UpdateAsync(id, phone, email, request.AvatarUrl,
            request.Name, request.LastName, request.Patronymic, request.Birthday, request.Profession,
            cancellationToken);

        if (updateResult.IsFailure)
            return updateResult;

        await _cache.RemoveAsync($"cache:users:{id}", cancellationToken);

        return Result.Success();
    }
}
