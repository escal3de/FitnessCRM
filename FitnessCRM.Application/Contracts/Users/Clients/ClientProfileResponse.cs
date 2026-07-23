using FitnessCRM.Application.Contracts.SeasonTickets;

namespace FitnessCRM.Application.Contracts.Users.Clients;

public record ClientProfileResponse(
    Guid Id,
    int? Height,
    int? Width,
    string? MedicalContraindications,
    List<SeasonTicketResponse> SeasonTickets,
    string? ParentsCommunication,
    DateTime CreatedAt);