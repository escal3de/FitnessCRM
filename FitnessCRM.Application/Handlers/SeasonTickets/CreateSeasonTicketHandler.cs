using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Application.Abstractions.UnitOfWork;
using FitnessCRM.Application.Contracts.SeasonTickets;
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

    public async Task<Result> HandleAsync(CreateSeasonTicketRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage)));

        var userResult = await _usersRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        var clientProfile = userResult.Value.ClientProfile;
        if (clientProfile is null)
            return Result.Failure("User does not have a client profile");

        var ticketResult = Domain.SeasonTicket.Create(
            request.Name,
            request.ActiveAt,
            request.ExpiresAt,
            request.RemainingWorkouts,
            request.Price,
            request.SchoolName,
            request.QrCodeLink);

        if (ticketResult.IsFailure)
            return Result.Failure(ticketResult.Error);

        var addResult = await _seasonTicketRepository.AddAsync(ticketResult.Value, clientProfile.Id, cancellationToken);
        if (addResult.IsFailure)
            return Result.Failure(addResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
