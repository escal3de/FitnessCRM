namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record SeasonTicketTemplateResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Status,
    string Type,
    int ValidityPeriod,
    int WorkoutAmount,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt);