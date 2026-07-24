using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Domain;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Directions;

public class UpdateDirectionHandler(
    IDirectionsRepository repository,
    IValidator<UpdateDirectionRequest> validator,
    ICurrentUserService service,
    IUnitOfWork unitOfWork)
{
    private readonly IDirectionsRepository _repository = repository;
    private readonly IValidator<UpdateDirectionRequest> _validator = validator;
    private readonly ICurrentUserService _service = service;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(Guid id, UpdateDirectionRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure("Invalid school id");

        var existing = await _repository.GetByIdAsync(id, schoolId.Value, cancellationToken);
        if (existing.IsFailure)
            return Result.Failure(existing.Error);

        var direction = Direction.Load(
            id,
            request.Name ?? existing.Value.Name,
            request.Description ?? existing.Value.Description,
            existing.Value.Status,
            existing.Value.TeacherProfiles,
            existing.Value.CreatedAt);

        if (request.TeacherProfileIds is not null)
        {
            var teachersResult = await _repository.GetTeacherProfilesByIdsAsync(request.TeacherProfileIds, cancellationToken);
            if (teachersResult.IsFailure)
                return Result.Failure(teachersResult.Error);

            direction.TeacherProfiles.Clear();
            foreach (var teacher in teachersResult.Value)
                direction.TeacherProfiles.Add(teacher);
        }

        var result = await _repository.UpdateAsync(direction, schoolId.Value, cancellationToken);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
