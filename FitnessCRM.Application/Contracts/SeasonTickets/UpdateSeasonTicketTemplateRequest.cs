namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record UpdateSeasonTicketTemplateRequest(
    string? Name,
    decimal? Price,
    string? Type,
    int? ValidityPeriod,
    int? WorkoutAmount,
    DateTime? StartDate,
    DateTime? EndDate);
