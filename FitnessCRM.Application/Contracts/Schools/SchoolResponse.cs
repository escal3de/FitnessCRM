using FitnessCRM.Application.Contracts.Cabinets;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Contracts.SeasonTickets;

namespace FitnessCRM.Application.Contracts.Schools;

public record SchoolResponse(
    Guid Id,
    string? AvatarUrl,
    string Name,
    string ContactEmail,
    string Status,
    List<CabinetResponse> Cabinets,
    List<DirectionResponse> Directions,
    List<SeasonTicketTemplateResponse> SeasonTicketsTemplates,
    DateTime CreatedAt);