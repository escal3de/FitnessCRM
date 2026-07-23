using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.SeasonTickets;

public class GetSeasonTicketHandler(
    ISeasonTicketRepository repository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly ISeasonTicketRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result<SeasonTicketResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"cache:season-tickets:{_current.SchoolId}:{id}";
        
        var cache = await _cache.GetAsync<SeasonTicketResponse>(cacheKey, cancellationToken);
        if (cache is not null)
            return Result.Success(cache);
        
        var seasonTicket = await _repository.GetByIdAsync(id, cancellationToken);
        if (seasonTicket.IsFailure)
            return Result.Failure<SeasonTicketResponse>(seasonTicket.Error);

        await _cache.SetAsync(cacheKey, seasonTicket.Value, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(seasonTicket.Value.Map());
    }
}
