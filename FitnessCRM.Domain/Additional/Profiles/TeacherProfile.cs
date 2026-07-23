using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums.Teacher;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Domain.Additional.Profiles;

public class TeacherProfile
{
    public Guid Id { get; private set; }
    public PositionStatus? PositionStatus { get; private set; }
    public List<Direction>? Directions { get; private set; }
    public Education? Education { get; private set; }
    public string? Achievements { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TeacherProfile(Guid id, PositionStatus? positionStatus, List<Direction>? directions, Education? education,
        string? achievements, DateTime createdAt)
    {
        Id = id;
        PositionStatus = positionStatus;
        Directions = directions;
        Education = education;
        Achievements = achievements;
        CreatedAt = createdAt;
    }

    public static Result<TeacherProfile> Create(PositionStatus? positionStatus = null, List<Direction>? directions = null,
        Education? education = null, string? achievements = null)
    {
        var validation = Validate(directions, achievements);
        if (validation.IsFailure)
            return Result.Failure<TeacherProfile>(validation.Error);

        return Result.Success(new TeacherProfile(Guid.NewGuid(), positionStatus, directions, education,
            achievements, DateTime.UtcNow));
    }

    private static Result Validate(List<Direction> directions, string? achievements)
    {
        if (directions is null || directions.Count == 0)
            return Result.Failure("At least one direction is required");

        if (achievements is not null && achievements.Length > 5000)
            return Result.Failure("Achievements must not exceed 5000 characters");

        return Result.Success();
    }

    public static TeacherProfile Load(Guid id, PositionStatus? positionStatus, List<Direction>? directions,
        Education? education, string? achievements, DateTime createdAt)
        => new TeacherProfile(id, positionStatus, directions, education, achievements, createdAt);
}