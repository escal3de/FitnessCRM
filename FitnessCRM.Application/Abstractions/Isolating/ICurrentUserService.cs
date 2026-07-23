using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Application.Abstractions.Isolating;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid? SchoolId { get; }
    UserRole UserRole { get; }
}
