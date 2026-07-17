using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Contracts.Educations;
using FitnessCRM.Application.Contracts.Schools;

namespace FitnessCRM.Application.Contracts.Users.Teachers;

public record TeacherProfileResponse(
    Guid Id,
    string PositionStatus,
    List<DirectionResponse> Directions,
    EducationResponse? Education,
    SchoolResponse? School,
    string? Achievements,
    DateTime CreatedAt);