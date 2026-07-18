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
        var validation = Validate(name, mediaUrls, description);
        if (validation.IsFailure)
            return Result.Failure<Cabinet>(validation.Error);

        return Result.Success(new Cabinet(Guid.NewGuid(), mediaUrls, name, description, Status.Active,
            DateTime.UtcNow));
    }

    private static Result Validate(string name, List<string> mediaUrls, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Cabinet name is required");

        if (name.Length > 200)
            return Result.Failure("Cabinet name must not exceed 200 characters");

        if (description is not null && description.Length > 2000)
            return Result.Failure("Description must not exceed 2000 characters");

        if (mediaUrls is null)
            return Result.Failure("Media URLs list cannot be null");

        foreach (var url in mediaUrls)
        {
            if (url.Length > 2048)
                return Result.Failure("Media URL must not exceed 2048 characters");
        }

        return Result.Success();
    }

    public static Cabinet Load(Guid id, List<string> mediaUrls, string name, string? description, Status status,
        DateTime createdAt)
        => new Cabinet(id, mediaUrls, name, description, status, createdAt);
}