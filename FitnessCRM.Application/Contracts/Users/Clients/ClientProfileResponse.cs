using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Contracts.Users.Clients;

public record ClientProfileResponse(
    Guid Id,
    int? Height,
    int? Width,
    string? MedicalContraindications,
    string Gender,
    List<string> Schools,
    List<SeasonTicketResponse> SeasonTickets,
    string? ParentsCommunication,
    DateTime CreatedAt);