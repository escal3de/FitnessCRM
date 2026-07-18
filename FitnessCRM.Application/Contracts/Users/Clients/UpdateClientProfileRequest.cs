namespace FitnessCRM.Application.Contracts.Users.Clients;

public record UpdateClientProfileRequest(
    int? Height,
    int? Weight,
    string? MedicalContraindications,
    string? ParentsCommunication);
