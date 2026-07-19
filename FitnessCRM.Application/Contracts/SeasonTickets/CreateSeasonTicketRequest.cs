namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record CreateSeasonTicketRequest(
    Guid UserId,
    string Name,
    DateTime ActiveAt,
    DateTime ExpiresAt,
    int RemainingWorkouts,
    decimal Price,
    string SchoolName);