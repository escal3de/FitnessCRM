using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums;

namespace FitnessCRM.Domain;

public class Direction
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Direction(Guid id, string name, string description, Status status, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Result<Direction> Create(string name, string description)
    {
        return Result.Success(new Direction(Guid.NewGuid(), name, description, Status.Active, DateTime.UtcNow));
    }

    public static Direction Load(Guid id, string name, string description, Status status, DateTime createdAt)
        => new Direction(id, name, description, status, createdAt);
}