using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Mappers.VO;

namespace FitnessCRM.Persistence.Mappers.Additional.Profiles;

public static class TeacherProfileMapper
{
    public static TeacherProfileEntity Map(this TeacherProfile profile, Guid userId)
        => new TeacherProfileEntity
        {
            Id = profile.Id,
            UserId = userId,
            PositionStatus = profile.PositionStatus,
            Directions = profile.Directions.Select(x => x.Map()).ToList(),
            Education = profile.Education?.Map(profile.Id),
            School = profile.School?.Map(),
            Achievements = profile.Achievements
        };

    public static TeacherProfile Map(this TeacherProfileEntity entity)
        => TeacherProfile.Load(
            entity.Id,
            entity.PositionStatus,
            entity.Directions.Select(x => x.Map()).ToList() ?? [],
            entity.Education?.Map(),
            entity.School.Map(),
            entity.Achievements);
}