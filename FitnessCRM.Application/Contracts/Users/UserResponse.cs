using FitnessCRM.Application.Contracts.Schools;

namespace FitnessCRM.Application.Contracts.Users;

public record UserResponse(
    Guid Id,
    string Phone,
    ContactDataResponse? ContactData, 
    SchoolResponse? School,
    string Role,
    bool IsTwoFactorEnabled,
    DateTime UpdatedAt,
    DateTime CreatedAt);