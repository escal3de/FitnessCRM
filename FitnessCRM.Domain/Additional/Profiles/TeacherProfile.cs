using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums.Teacher;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Domain.Additional.Profiles;

public class TeacherProfile
{
    public Guid Id { get; private set; }
    public PositionStatus PositionStatus { get; private set; }
    public List<Direction> Directions { get; private set; }
    public Education? Education { get; private set; }
    public School? School { get; private set; }
    public string? Achievements { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public TeacherProfile(Guid id, PositionStatus positionStatus, List<Direction> directions, Education? education,
        School? school, string? achievements, DateTime createdAt)
    {
        Id = id;
        PositionStatus = positionStatus;
        Directions = directions;
        Education = education;
        School = school;
        Achievements = achievements;
        CreatedAt = createdAt;
    }

    public static Result<TeacherProfile> Create(PositionStatus positionStatus, List<Direction> directions,
        Education? education, School? school, string? achievements)
    {
        return Result.Success(new TeacherProfile(Guid.NewGuid(), positionStatus, directions, education, school,
            achievements, DateTime.UtcNow));
    }

    public static TeacherProfile Load(Guid id, PositionStatus positionStatus, List<Direction> directions,
        Education? education,
        School? school, string? achievements, DateTime createdAt)
        => new TeacherProfile(id, positionStatus, directions, education, school, achievements, createdAt);
}