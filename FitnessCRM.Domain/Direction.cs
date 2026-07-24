using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Domain.Enums;

namespace FitnessCRM.Domain;

public class Direction
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; }
    public List<TeacherProfile> TeacherProfiles { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Direction(Guid id, string name, string description, Status status, List<TeacherProfile> teacherProfiles,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
        TeacherProfiles = teacherProfiles;
        CreatedAt = createdAt;
    }

    public static Result<Direction> Create(string name, string description)
    {
        var validation = Validate(name, description);
        if (validation.IsFailure)
            return Result.Failure<Direction>(validation.Error);

        return Result.Success(new Direction(Guid.NewGuid(), name, description, Status.Active, [], DateTime.UtcNow));
    }

    private static Result Validate(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Direction name is required");

        if (name.Length > 200)
            return Result.Failure("Direction name must not exceed 200 characters");

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure("Direction description is required");

        if (description.Length > 2000)
            return Result.Failure("Direction description must not exceed 2000 characters");

        return Result.Success();
    }

    public static Direction Load(Guid id, string name, string description, Status status,
        List<TeacherProfile> teacherProfiles, DateTime createdAt)
        => new Direction(id, name, description, status, teacherProfiles, createdAt);
}