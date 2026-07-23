using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Mappers.Additional.Profiles;

public static class StaffProfileMapper
{
    public static StaffProfileEntity Map(this StaffProfile profile, Guid userId)
        => new StaffProfileEntity
        {
            Id = profile.Id,
            UserId = userId,
            CreatedAt = profile.CreatedAt
        };

    public static StaffProfile Map(this StaffProfileEntity entity)
        => StaffProfile.Load(
            entity.Id,
            entity.CreatedAt);
}
