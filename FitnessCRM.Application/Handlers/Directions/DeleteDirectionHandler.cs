using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Domain;

namespace FitnessCRM.Application.Handlers.Directions;

public class DeleteDirectionHandler(
    IDirectionsRepository repository,
    ICurrentUserService service,
    IUnitOfWork unitOfWork)
{
    private readonly IDirectionsRepository _repository = repository;
    private readonly ICurrentUserService _service = service;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure("Invalid school id");

        var existing = await _repository.GetByIdAsync(id, schoolId.Value, cancellationToken);
        if (existing.IsFailure)
            return Result.Failure(existing.Error);

        var result = await _repository.DeleteAsync(existing.Value, schoolId.Value, cancellationToken);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
