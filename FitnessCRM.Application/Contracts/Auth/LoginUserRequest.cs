namespace FitnessCRM.Application.Contracts.Auth;

public record LoginUserRequest(
    string Phone,
    string Password);