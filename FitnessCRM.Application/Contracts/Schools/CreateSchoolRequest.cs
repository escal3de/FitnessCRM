namespace FitnessCRM.Application.Contracts.Schools;

public record CreateSchoolRequest(
    string Name,
    string Email,
    string? AvatarUrl);
