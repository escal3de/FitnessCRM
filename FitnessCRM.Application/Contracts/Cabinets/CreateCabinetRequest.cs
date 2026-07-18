namespace FitnessCRM.Application.Contracts.Cabinets;

public record CreateCabinetRequest(
    string Name,
    string? Description,
    List<string> MediaUrls);
