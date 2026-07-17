using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Mappers;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Handlers.Users;

public class GetUserHandler(
    IUsersRepository repository,
    ICacheService cache)
{
    private readonly IUsersRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    public async Task<Result<object>> HandleAsync(Guid id,
        CancellationToken cancellationToken, Guid? checkerId = null)
    {
        var cache = await _cache.GetAsync<object>($"cache:users:{id}", cancellationToken);
        if (cache != null)
            return Result.Success(cache);
        
        var userResult = await _repository.GetByIdAsync(id, cancellationToken);
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        if (checkerId.HasValue)
        {
            var checkerUser = await _repository.GetByIdAsync(checkerId.Value, cancellationToken);
            if (checkerUser.IsFailure)
                return Result.Failure<object>(checkerUser.Error);

            if ((int)checkerUser.Value.Role > (int)userResult.Value.Role)
                return Result.Failure("You cannot view this user: their role is higher than yours");
        }

        var user = userResult.Value;

        object result = user.Role switch
        {
            _ when user.Role is UserRole.Default => user.Map().ToClientResponse(user.ClientProfile),
            _ when user.Role is UserRole.Teacher => user.Map().ToTeacherResponse(user.TeacherProfile),
            _ when user.Role is UserRole.Staff => user.Map().ToStaffResponse(user.StaffProfile),
            _ => null
        };
        
        if (result is null)
            return Result.Failure<object>($"Cannot resolve profile for role '{user.Role}'");

        await _cache.SetAsync($"cache:users:{id}", result, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(result);
    }
}