using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Isolating;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Domain.Enums.SeasonTicket;
using FitnessCRM.Domain.Templates;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.SeasonTicketTemplates;

public class CreateSeasonTicketTemplateHandler(
    ISeasonTicketTemplatesRepository repository,
    IValidator<CreateSeasonTicketTemplateRequest> validator,
    ICurrentUserService current,
    IUnitOfWork unitOfWork)
{
    private readonly ISeasonTicketTemplatesRepository _repository = repository;
    private readonly IValidator<CreateSeasonTicketTemplateRequest> _validator = validator;
    private readonly ICurrentUserService _current = current;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(CreateSeasonTicketTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        if (!Enum.TryParse(request.Type, out TicketType type))
            return Result.Failure("Invalid ticket type");

        var seasonTicketTemplate = SeasonTicketTemplate.Create(
            request.Name,
            request.Price,
            type,
            request.ValidityPeriod, 
            request.WorkoutAmount,
            request.StartDate,
            request.EndDate);
        if (seasonTicketTemplate.IsFailure)
            return Result.Failure(seasonTicketTemplate.Error);

        var schoolId = _current.SchoolId;
        if (!schoolId.HasValue)
            return Result.Failure("Invalid school id");

        await _repository.AddAsync(seasonTicketTemplate.Value, schoolId.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}