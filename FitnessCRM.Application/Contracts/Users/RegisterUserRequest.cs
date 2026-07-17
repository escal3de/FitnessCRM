namespace FitnessCRM.Application.Contracts.Users;

public record RegisterUserRequest(
    string Phone,
    string Password);