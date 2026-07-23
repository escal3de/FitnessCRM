using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;

namespace FitnessCRM.Application.Handlers.SeasonTicketTemplates;

public class DeleteSeasonTicketTemplateHandler(
    ISeasonTicketTemplatesRepository repository,
    ICacheService cache,
    ICurrentUserService service)
{
    private readonly ISeasonTicketTemplatesRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly ICurrentUserService _service = service;

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure("Invalid school id");

        var seasonTicketTemplate = await _repository.GetByIdAsync(id, schoolId.Value, cancellationToken);
        if (seasonTicketTemplate.IsFailure)
            return Result.Failure(seasonTicketTemplate.Error);

        await _cache.RemoveAsync($"cache:season-ticket-template:{id}", cancellationToken);
        await _cache.RemoveAsync($"cache:season-ticket-templates:all:{schoolId.Value}", cancellationToken);

        await _repository.DeleteAsync(id, schoolId.Value, cancellationToken);

        return Result.Success();
    }
}