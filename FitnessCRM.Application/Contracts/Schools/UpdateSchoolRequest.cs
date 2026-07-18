namespace FitnessCRM.Application.Contracts.Schools;

public record UpdateSchoolRequest(
    string? Name,
    string? Email,
    string? AvatarUrl);
