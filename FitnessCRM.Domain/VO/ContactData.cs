using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Domain.VO;

public sealed class ContactData
{
    public string? AvatarUrl { get; }
    public Email? Email { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Patronymic { get; } // отчество лол хвахвха
    public DateTime? Birthday { get; }
    public UserGender UserGender { get; private set; }
    public string? Profession { get; }
    public DateTime LastUpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private ContactData(string? avatarUrl, Email? email, string name, string lastName, string patronymic,
        DateTime? birthday, UserGender userGender, string? profession, DateTime lastUpdatedAt, DateTime createdAt)
    {
        AvatarUrl = avatarUrl;
        Email = email;
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;
        Birthday = birthday;
        UserGender = userGender;
        Profession = profession;
        LastUpdatedAt = lastUpdatedAt;
        CreatedAt = createdAt;
    }

    public static Result<ContactData> Create(string? avatarUrl, Email? email,
        string name, string lastName, string patronymic, UserGender gender)
    {
        var validation = Validate(name, lastName, patronymic, avatarUrl);
        if (validation.IsFailure)
            return Result.Failure<ContactData>(validation.Error);

        return Result.Success(new ContactData(avatarUrl, email, name, lastName, patronymic, null, gender, null,
            DateTime.UtcNow, DateTime.UtcNow));
    }

    private static Result Validate(string name, string lastName, string patronymic, string? avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Name is required");

        if (name.Length > 100)
            return Result.Failure("Name must not exceed 100 characters");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure("Last name is required");

        if (lastName.Length > 100)
            return Result.Failure("Last name must not exceed 100 characters");

        if (string.IsNullOrWhiteSpace(patronymic))
            return Result.Failure("Patronymic is required");

        if (patronymic.Length > 100)
            return Result.Failure("Patronymic must not exceed 100 characters");

        if (avatarUrl is not null && avatarUrl.Length > 2048)
            return Result.Failure("Avatar URL must not exceed 2048 characters");

        return Result.Success();
    }

    public static ContactData Load(string? avatarUrl, Email? email, string name, string lastName, string patronymic,
        DateTime? birthday, UserGender gender, string? profession, DateTime lastUpdatedAt, DateTime createdAt)
        => new ContactData(avatarUrl, email, name, lastName, patronymic, birthday, gender, profession, lastUpdatedAt,
            createdAt);
}