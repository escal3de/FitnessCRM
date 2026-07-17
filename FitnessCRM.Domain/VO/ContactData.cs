using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain.VO;

public sealed class ContactData
{
    public string? AvatarUrl { get; }
    public Email? Email { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Patronymic { get; } // отчество лол хвахвха
    public DateTime? Birthday { get; }
    public string? Profession { get; }
    public DateTime LastUpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private ContactData(string? avatarUrl, Email? email, string name, string lastName, string patronymic,
        DateTime? birthday, string? profession, DateTime lastUpdatedAt, DateTime createdAt)
    {
        AvatarUrl = avatarUrl;
        Email = email;
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;
        Birthday = birthday;
        Profession = profession;
        LastUpdatedAt = lastUpdatedAt;
        CreatedAt = createdAt;
    }

    public static Result<ContactData> Create(string? avatarUrl, Email? email,
        string name, string lastName, string patronymic)
    {
        return Result.Success(new ContactData(avatarUrl, email, name, lastName, patronymic, null, null,
            DateTime.UtcNow, DateTime.UtcNow));
    }

    public static ContactData Load(string? avatarUrl, Email? email, string name, string lastName, string patronymic,
        DateTime? birthday, string? profession, DateTime lastUpdatedAt, DateTime createdAt)
        => new ContactData(avatarUrl, email, name, lastName, patronymic, birthday, profession, lastUpdatedAt,
            createdAt);
}