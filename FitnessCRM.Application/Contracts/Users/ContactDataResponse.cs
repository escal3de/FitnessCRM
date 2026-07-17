namespace FitnessCRM.Application.Contracts.Users;

public record ContactDataResponse(
    string? AvatarUrl,
    string? Email,
    string Name,
    string LastName,
    string Patronymic,
    DateTime? Birthday,
    string? Profession,
    DateTime LastUpdateAt,
    DateTime CreatedAt);