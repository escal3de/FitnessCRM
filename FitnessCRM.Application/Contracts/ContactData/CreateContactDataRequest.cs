using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Contracts.ContactData;

public record CreateContactDataRequest(
    string Phone, 
    string? AvatarUrl,
    string Name,
    string LastName,
    string Patronymic,
    DateTime? Birthday,
    UserGender UserGender,
    string? Profession);