namespace FitnessCRM.Application.Contracts.Auth;

public record RegisterUserRequest(
    string Phone,
    string Password);