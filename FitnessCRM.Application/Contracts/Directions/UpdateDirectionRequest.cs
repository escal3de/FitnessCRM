namespace FitnessCRM.Application.Contracts.Directions;

public record UpdateDirectionRequest(
    string? Name,
    string? Description,
    List<Guid>? TeacherProfileIds);
