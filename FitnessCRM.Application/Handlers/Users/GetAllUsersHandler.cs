using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Mappers;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Handlers.Users;

public class GetAllUsersHandler(
    IUsersRepository repository,
    ICacheService cache)
{
    private readonly IUsersRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    public async Task<Result<IEnumerable<object>>> HandleAsync(string target, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(target, out UserRole role))
            return Result.Failure<IEnumerable<object>>("Invalid target");

        var cacheKey = $"cache:users:all:{role}";

        var cached = await _cache.GetAsync<IEnumerable<User>>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(MapUsers(role, cached));

        var result = await GetTargetUsers(role, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<IEnumerable<object>>(result.Error);

        await _cache.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(MapUsers(role, result.Value));
    }

    private IEnumerable<object> MapUsers(UserRole role, IEnumerable<User> users) => role switch
    {
        UserRole.Default => users.Select(x => x.Map().ToClientResponse(x.ClientProfile!)),
        UserRole.Teacher => users.Select(x => x.Map().ToTeacherResponse(x.TeacherProfile!)),
        UserRole.Staff => users.Select(x => x.StaffProfile!.Map()),
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported role")
    };

    private async Task<Result<IEnumerable<User>>> GetTargetUsers(UserRole role, CancellationToken cancellationToken)
        => role switch
        {
            UserRole.Default => await _repository.GetAllClientsAsync(cancellationToken),
            UserRole.Teacher => await _repository.GetAllTeachersAsync(cancellationToken),
            UserRole.Staff => await _repository.GetAllStaffAsync(cancellationToken),
            _ => Result.Failure<IEnumerable<User>>($"Unsupported target: {role}")
        };
}