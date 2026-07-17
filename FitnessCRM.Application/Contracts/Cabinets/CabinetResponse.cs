namespace FitnessCRM.Application.Contracts.Cabinets;

public record CabinetResponse(
    Guid Id,
    List<string> MediaUrls,
    string Name,
    string? Description,
    string Status,
    DateTime CreatedAt);