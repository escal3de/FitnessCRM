using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Domain;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.Directions;

public class CreateDirectionHandler(
    IDirectionsRepository repository,
    IValidator<CreateDirectionRequest> validator,
    ICurrentUserService service,
    IUnitOfWork unitOfWork)
{
    private readonly IDirectionsRepository _repository = repository;
    private readonly IValidator<CreateDirectionRequest> _validator = validator;
    private readonly ICurrentUserService _service = service;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(CreateDirectionRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var schoolId = _service.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure("Invalid school id");

        var direction = Direction.Create(request.Name, request.Description);
        if (direction.IsFailure)
            return Result.Failure(direction.Error);

        if (request.TeacherProfileIds is { Count: > 0 })
        {
            var teachersResult = await _repository.GetTeacherProfilesByIdsAsync(request.TeacherProfileIds, cancellationToken);
            if (teachersResult.IsFailure)
                return Result.Failure(teachersResult.Error);

            foreach (var teacher in teachersResult.Value)
                direction.Value.TeacherProfiles.Add(teacher);
        }

        await _repository.AddAsync(direction.Value, schoolId.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
