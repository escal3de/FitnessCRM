using FitnessCRM.Application.Contracts.Users.Teachers;

namespace FitnessCRM.Application.Contracts.Directions;

public record DirectionResponse(
    Guid Id,
    string Name,
    string Description,
    string Status,
    List<TeacherProfileResponse> TeacherProfiles,
    DateTime CreatedAt);