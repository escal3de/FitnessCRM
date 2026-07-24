using FitnessCRM.Domain;
using FitnessCRM.Persistence.Entities;
using FitnessCRM.Persistence.Mappers.Additional.Profiles;

namespace FitnessCRM.Persistence.Mappers;

public static class DirectionMapper
{
    public static DirectionEntity Map(this Direction direction, Guid? schoolId = null)
        => new DirectionEntity
        {
            Id = direction.Id,
            SchoolId = schoolId ?? Guid.Empty,
            Name = direction.Name,
            Description = direction.Description,
            Status = direction.Status,
            CreatedAt = direction.CreatedAt
        };

    public static Direction Map(this DirectionEntity entity)
        => Direction.Load(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Status,
            entity.TeacherProfiles?.Select(x => x.Map()).ToList() ?? [],
            entity.CreatedAt);
}
