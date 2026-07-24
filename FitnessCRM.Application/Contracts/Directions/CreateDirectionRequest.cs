namespace FitnessCRM.Application.Contracts.Directions;

public record CreateDirectionRequest(
    string Name,
    string Description,
    List<Guid>? TeacherProfileIds);
