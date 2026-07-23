using FitnessCRM.Application.Contracts.ContactData;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Contracts.Users;

public record CreateUserRequest(
    string Phone, 
    CreateContactDataRequest ContactData,
    UserRole Role,
    string Password);