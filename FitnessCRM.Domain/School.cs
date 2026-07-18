using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Additional.Other;
using FitnessCRM.Domain.Enums;
using FitnessCRM.Domain.Templates;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Domain;

public class School
{
    public Guid Id { get; private set; }
    public string? AvatarUrl { get; private set; }
    public string Name { get; private set; }
    public Email ContactEmail { get; private set; }
    public Status Status { get; private set; }
    public List<Cabinet> Cabinets { get; private set; }
    public List<Direction> Directions { get; private set; }
    public List<SeasonTicketTemplate> SeasonTicketTemplates { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private School(Guid id, string? avatarUrl, string name, Email contactEmail, Status status, List<Cabinet> cabinets,
        List<Direction> directions, List<SeasonTicketTemplate> seasonTicketTemplates, DateTime createdAt)
    {
        Id = id;
        AvatarUrl = avatarUrl;
        Name = name;
        ContactEmail = contactEmail;
        Status = status;
        Cabinets = cabinets;
        Directions = directions;
        SeasonTicketTemplates = seasonTicketTemplates;
        CreatedAt = createdAt;
    }

    public static Result<School> Create(string? avatarUrl, string name, Email contactEmail)
    {
        var validation = Validate(name, avatarUrl);
        if (validation.IsFailure)
            return Result.Failure<School>(validation.Error);

        return Result.Success(new School(Guid.NewGuid(), avatarUrl, name, contactEmail, Status.Active,
            [], [], [], DateTime.UtcNow));
    }

    private static Result Validate(string name, string? avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("School name is required");

        if (name.Length > 200)
            return Result.Failure("School name must not exceed 200 characters");

        if (avatarUrl is not null && avatarUrl.Length > 2048)
            return Result.Failure("Avatar URL must not exceed 2048 characters");

        return Result.Success();
    }

    public static School Load(Guid id, string? avatarUrl, string name, Email contactEmail, Status status,
        List<Cabinet> cabinets, List<Direction> directions, List<SeasonTicketTemplate> seasonTicketTemplates,
        DateTime createdAt)
        => new School(id, avatarUrl, name, contactEmail, status, cabinets, directions, seasonTicketTemplates,
            createdAt);
}