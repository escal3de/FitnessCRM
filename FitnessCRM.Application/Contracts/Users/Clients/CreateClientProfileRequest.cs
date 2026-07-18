namespace FitnessCRM.Application.Contracts.Users.Clients;

public record CreateClientProfileRequest(
    int? Height,
    int? Weight);
