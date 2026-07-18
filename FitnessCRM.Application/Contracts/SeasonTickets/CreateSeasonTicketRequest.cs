namespace FitnessCRM.Application.Contracts.SeasonTickets;

public record CreateSeasonTicketRequest(
    Guid ClientId,
    string Name,
    DateTime ActiveAt,
    DateTime ExpiresAt,
    int RemainingWorkouts,
    decimal Price,
    string SchoolName,
    string QrCodeLink);