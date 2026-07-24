using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.Directions;

public class GetAllDirectionsHandler(
    IDirectionsRepository repository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly IDirectionsRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result<IEnumerable<DirectionResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        if (_current.SchoolId is null)
            return Result.Failure<IEnumerable<DirectionResponse>>("School id is required");

        var cacheKey = $"cache:directions:{_current.SchoolId.Value}:all";

        var cached = await _cache.GetAsync<IEnumerable<DirectionResponse>>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(cached);

        var result = await _repository.GetAllAsync(_current.SchoolId.Value, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<IEnumerable<DirectionResponse>>(result.Error);

        var response = result.Value.Select(x => x.Map()).ToList();
        await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(response.AsEnumerable());
    }
}
