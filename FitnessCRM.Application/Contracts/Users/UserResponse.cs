namespace FitnessCRM.Application.Contracts.Users;

public record UserResponse(
    Guid Id,
    string Phone,
    ContactDataResponse? ContactData, 
    string Role,
    bool IsTwoFactorEnabled,
    DateTime UpdatedAt,
    DateTime CreatedAt);