namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record SeasonTicketResponse(
    Guid Id,
    string Name,
    DateTime ActiveAt,
    DateTime ExpiresAt,
    int RemainingWorkouts,
    decimal Price,
    string SchoolName,
    DateTime CreatedAt);