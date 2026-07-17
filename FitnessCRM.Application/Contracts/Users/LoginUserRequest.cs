namespace FitnessCRM.Application.Contracts.Users;

public record LoginUserRequest(
    string Phone,
    string Password);