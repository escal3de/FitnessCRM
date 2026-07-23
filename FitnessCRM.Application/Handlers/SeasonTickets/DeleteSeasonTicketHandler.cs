using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;

namespace FitnessCRM.Application.Handlers.SeasonTickets;

public class DeleteSeasonTicketHandler(
    ISeasonTicketRepository repository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly ISeasonTicketRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var seasonTicket = await _repository.GetByIdAsync(id, cancellationToken);
        if (seasonTicket.IsFailure)
            return Result.Failure(seasonTicket.Error);

        await _cache.RemoveAsync($"cache:season-tickets:{_current.SchoolId}:{id}", cancellationToken);
        
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
