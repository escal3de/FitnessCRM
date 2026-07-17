namespace FitnessCRM.Application.Contracts.Users;

public record UpdateUserRequest(
    string? Phone,
    string? AvatarUrl,
    string? Email,
    string? Name,
    string? LastName,
    string? Patronymic,
    DateTime? Birthday,
    string? Profession);
