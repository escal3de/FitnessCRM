namespace FitnessCRM.Application.Contracts.Auth;

public record LoginUserResponse(
    string Token,
    string RefreshToken,
    string Phone);