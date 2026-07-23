using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Mappers;

namespace FitnessCRM.Application.Handlers.SeasonTicketTemplates;

public class GetSeasonTicketTemplateHandler(
    ISeasonTicketTemplatesRepository repository,
    ICacheService cache,
    ICurrentUserService service)
{
    private readonly ISeasonTicketTemplatesRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _service = service;

    public async Task<Result<SeasonTicketTemplateResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure<SeasonTicketTemplateResponse>("Invalid school id");

        var cacheKey = $"cache:season-ticket-template:{id}";
        var cached = await _cache.GetAsync<SeasonTicketTemplateResponse>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(cached);
        
        var seasonTicketTemplate = await _repository.GetByIdAsync(id, schoolId.Value, cancellationToken);
        if (seasonTicketTemplate.IsFailure)
            return Result.Failure<SeasonTicketTemplateResponse>(seasonTicketTemplate.Error);
        
        var result = seasonTicketTemplate.Value.Map();
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(2), cancellationToken);
        
        return Result.Success(result);
    }
}