using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.SeasonTickets;

public class GetAllSeasonTicketsHandler(
    ISeasonTicketRepository seasonTicketRepository,
    IUsersRepository usersRepository,
    ICacheService cache,
    ICurrentUserService current)
{
    private readonly ISeasonTicketRepository _seasonTicketRepository = seasonTicketRepository;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _current = current;

    public async Task<Result<IEnumerable<SeasonTicketResponse>>> HandleAsync(CancellationToken cancellationToken,
        Guid? clientId = null, Guid? userId = null)
    {
        if (clientId.HasValue && userId.HasValue)
            return Result.Failure<IEnumerable<SeasonTicketResponse>>(
                "Failed to season tickets: it is not possible to find season tickets using userId and clientId");

        if (clientId.HasValue)
        {
            var cacheKey = $"cache:season-tickets:{_current.SchoolId}:{clientId.Value}";

            var cached = await _cache.GetAsync<IEnumerable<SeasonTicketResponse>>(cacheKey, cancellationToken);
            if (cached is not null)
                return Result.Success(cached);

            var result = await _seasonTicketRepository.GetAllByClientProfileIdAsync(clientId.Value, cancellationToken);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<SeasonTicketResponse>>(result.Error);

            var response = result.Value.Select(x => x.Map()).ToList();
            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(2), cancellationToken);

            return Result.Success(response.AsEnumerable());
        }

        if (userId.HasValue)
        {
            var userCacheKey = $"cache:season-tickets:{_current.SchoolId}:user:{userId.Value}";
            
            var cached = await _cache.GetAsync<IEnumerable<SeasonTicketResponse>>(userCacheKey, cancellationToken);
            if (cached is not null)
                return Result.Success(cached);
            
            var user = await _usersRepository.GetByIdAsync(userId.Value, _current.SchoolId, cancellationToken);
            if (user.IsFailure)
                return Result.Failure<IEnumerable<SeasonTicketResponse>>(user.Error);

            var clientProfile = user.Value.ClientProfile;
            if (clientProfile is null)
                return Result.Failure<IEnumerable<SeasonTicketResponse>>("Client profile is empty");
            
            var result = await _seasonTicketRepository.GetAllByClientProfileIdAsync(clientProfile.Id, cancellationToken);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<SeasonTicketResponse>>(result.Error);
            
            var response = result.Value.Select(x => x.Map()).ToList();
            await _cache.SetAsync(userCacheKey, response, TimeSpan.FromMinutes(2), cancellationToken);
            
            return Result.Success(response.AsEnumerable());
        }

        if (_current.SchoolId is null)
            return Result.Failure<IEnumerable<SeasonTicketResponse>>("School id is required");

        var allCacheKey = $"cache:season-tickets:{_current.SchoolId.Value}:all";

        var allCached = await _cache.GetAsync<IEnumerable<SeasonTicketResponse>>(allCacheKey, cancellationToken);
        if (allCached is not null)
            return Result.Success(allCached);

        var allResult = await _seasonTicketRepository.GetAllBySchoolIdAsync(_current.SchoolId.Value, cancellationToken);
        if (allResult.IsFailure)
            return Result.Failure<IEnumerable<SeasonTicketResponse>>(allResult.Error);

        var allResponse = allResult.Value.Select(x => x.Map()).ToList();
        await _cache.SetAsync(allCacheKey, allResponse, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(allResponse.AsEnumerable());
    }
}
