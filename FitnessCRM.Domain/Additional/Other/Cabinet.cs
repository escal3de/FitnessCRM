using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums;

namespace FitnessCRM.Domain.Additional.Other;

public class Cabinet
{
    public Guid Id { get; private set; }
    public List<string> MediaUrls { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Cabinet(Guid id, List<string> mediaUrls, string name, string? description, Status status,
        DateTime createdAt)
    {
        Id = id;
        MediaUrls = mediaUrls;
        Name = name;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Result<Cabinet> Create(List<string> mediaUrls, string name, string? description)
    {
        return Result.Success(new Cabinet(Guid.NewGuid(), mediaUrls, name, description, Status.Active,
            DateTime.UtcNow));
    }

    public static Cabinet Load(Guid id, List<string> mediaUrls, string name, string? description, Status status,
        DateTime createdAt)
        => new Cabinet(id, mediaUrls, name, description, status, createdAt);
}