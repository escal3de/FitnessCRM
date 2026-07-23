using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.SeasonTicketTemplates;

public class GetAllSeasonTicketTemplatesHandler(
    ISeasonTicketTemplatesRepository repository,
    ICacheService cache,
    ICurrentUserService service)
{
    private readonly ISeasonTicketTemplatesRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _service = service;

    public async Task<Result<IEnumerable<SeasonTicketTemplateResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure<IEnumerable<SeasonTicketTemplateResponse>>("Invalid school id");

        var cacheKey = $"cache:season-ticket-templates:all:{schoolId.Value}";
        var cached = await _cache.GetAsync<IEnumerable<SeasonTicketTemplateResponse>>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(cached);
        
        var seasonTicketTemplates = await _repository.GetAllBySchoolAsync(schoolId.Value, cancellationToken);
        if (seasonTicketTemplates.IsFailure)
            return Result.Failure<IEnumerable<SeasonTicketTemplateResponse>>(seasonTicketTemplates.Error);

        var map = seasonTicketTemplates.Value.Select(x => x.Map());
        
        await _cache.SetAsync(cacheKey, map, TimeSpan.FromMinutes(2), cancellationToken);

        return Result.Success(map);
    }
}