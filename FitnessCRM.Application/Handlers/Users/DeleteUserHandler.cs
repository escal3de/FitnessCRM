using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Repositories;

namespace FitnessCRM.Application.Handlers.Users;

public class DeleteUserHandler(
    IUsersRepository repository,
    ICacheService cache)
{
    private readonly IUsersRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    public async Task<Result> HandleAsync(Guid id, Guid deleterId, CancellationToken cancellationToken)
    {
        var userResult = await _repository.GetByIdAsync(id, cancellationToken);
        if (userResult.IsFailure)
            return userResult;

        var deleterResult = await _repository.GetByIdAsync(deleterId, cancellationToken);
        if (deleterResult.IsFailure)
            return deleterResult;

        if ((int)userResult.Value.Role > (int)deleterResult.Value.Role)
            return Result.Failure("You cannot delete this user: their role is higher than yours");

        await _cache.RemoveAsync($"cache:users:{id}", cancellationToken);

        return await _repository.DeleteAsync(id, cancellationToken);
    }
}