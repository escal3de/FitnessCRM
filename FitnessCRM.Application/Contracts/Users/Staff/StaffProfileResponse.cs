using FitnessCRM.Application.Contracts.Schools;

namespace FitnessCRM.Application.Contracts.Users.Staff;

public record StaffProfileResponse(
    Guid Id,
    List<SchoolResponse>? Schools, 
    DateTime CreatedAt);