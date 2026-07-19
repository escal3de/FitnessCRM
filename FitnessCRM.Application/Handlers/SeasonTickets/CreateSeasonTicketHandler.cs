using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Mappers;
using FitnessCRM.Domain;
using FluentValidation;

namespace FitnessCRM.Application.Handlers.SeasonTickets;

public class CreateSeasonTicketHandler(
    ISeasonTicketRepository seasonTicketRepository,
    IUsersRepository usersRepository,
    IValidator<CreateSeasonTicketRequest> validator,
    IUnitOfWork unitOfWork)
{
    private readonly ISeasonTicketRepository _seasonTicketRepository = seasonTicketRepository;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IValidator<CreateSeasonTicketRequest> _validator = validator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SeasonTicketResponse>> HandleAsync(CreateSeasonTicketRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure<SeasonTicketResponse>(
                string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user.IsFailure)
            return Result.Failure<SeasonTicketResponse>(user.Error);

        if (user.Value.ClientProfile is null)
            return Result.Failure<SeasonTicketResponse>("Client profile is empty");

        var seasonTicket = SeasonTicket.Create(
            request.Name,
            request.ActiveAt,
            request.ExpiresAt,
            request.RemainingWorkouts,
            request.Price,
            request.SchoolName);
        if (seasonTicket.IsFailure)
            return Result.Failure<SeasonTicketResponse>(seasonTicket.Error);

        var result = await _seasonTicketRepository.AddAsync(seasonTicket.Value, user.Value.ClientProfile.Id, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<SeasonTicketResponse>(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(seasonTicket.Value.Map());
    }
}