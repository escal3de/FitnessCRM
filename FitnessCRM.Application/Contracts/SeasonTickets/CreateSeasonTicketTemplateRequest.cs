namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record CreateSeasonTicketTemplateRequest(
    string Name,
    decimal Price,
    string Type,
    int ValidityPeriod,
    int WorkoutAmount,
    DateTime? StartDate,
    DateTime EndDate);
