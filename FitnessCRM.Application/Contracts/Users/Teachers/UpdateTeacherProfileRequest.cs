namespace FitnessCRM.Application.Contracts.Users.Teachers;

public record UpdateTeacherProfileRequest(
    string? PositionStatus,
    List<Guid>? DirectionIds,
    string? Achievements);
