using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;

namespace FitnessCRM.Application.Handlers.Users;

public class DeleteUserHandler(
    IUsersRepository repository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly IUsersRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var userResult = await _repository.GetByIdAsync(id, _current.SchoolId, cancellationToken);
        if (userResult.IsFailure)
            return userResult;

        if (_current.SchoolId is not null && userResult.Value.School?.Id != _current.SchoolId)
            return Result.Failure("User does not belong to your school");

        if ((int)userResult.Value.Role > (int)_current.UserRole)
            return Result.Failure("You cannot delete this user: their role is higher than yours");

        await _cache.RemoveAsync($"cache:users:{_current.SchoolId}:{id}", cancellationToken);

        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
