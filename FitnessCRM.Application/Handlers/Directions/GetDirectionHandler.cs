using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.Directions;

public class GetDirectionHandler(
    IDirectionsRepository repository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly IDirectionsRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result<DirectionResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        if (_current.SchoolId is null)
            return Result.Failure<DirectionResponse>("School id is required");

        var cacheKey = $"cache:directions:{_current.SchoolId.Value}:{id}";

        var cached = await _cache.GetAsync<DirectionResponse>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(cached);

        var result = await _repository.GetByIdAsync(id, _current.SchoolId.Value, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<DirectionResponse>(result.Error);

        var response = result.Value.Map();
        await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(response);
    }
}
