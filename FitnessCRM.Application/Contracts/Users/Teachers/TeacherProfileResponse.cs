using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Contracts.Educations;

namespace FitnessCRM.Application.Contracts.Users.Teachers;

public record TeacherProfileResponse(
    Guid Id,
    string PositionStatus,
    List<DirectionResponse> Directions,
    EducationResponse? Education,
    string? Achievements,
    DateTime CreatedAt);