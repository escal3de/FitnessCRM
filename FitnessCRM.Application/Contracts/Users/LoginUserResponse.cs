namespace FitnessCRM.Application.Contracts.Users;

public record LoginUserResponse(
    string Token,
    string RefreshToken,
    string Phone);