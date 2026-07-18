namespace FitnessCRM.Application.Contracts.Users.Teachers;

public record CreateTeacherProfileRequest(
    string PositionStatus,
    List<Guid> DirectionIds,
    string? Achievements);
