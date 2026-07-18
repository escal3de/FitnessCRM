namespace FitnessCRM.Application.Contracts.Cabinets;

public record UpdateCabinetRequest(
    string? Name,
    string? Description,
    List<string>? MediaUrls);
