namespace FitnessCRM.Application.Contracts.Directions;

public record DirectionResponse(
    Guid Id,
    string Name,
    string Description,
    string Status,
    DateTime CreatedAt);