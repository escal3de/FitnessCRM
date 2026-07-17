using FitnessCRM.Domain.Additional.Other;
using FitnessCRM.Persistence.Entities.Additional.Other;

namespace FitnessCRM.Persistence.Mappers.Additional.Other;

public static class CabinetMapper
{
    public static CabinetEntity Map(this Cabinet cabinet, Guid? schoolId = null)
        => new CabinetEntity
        {
            Id = cabinet.Id,
            SchoolId = schoolId ?? Guid.Empty,
            MediaUrls = cabinet.MediaUrls,
            Name = cabinet.Name,
            Description = cabinet.Description,
            Status = cabinet.Status,
            CreatedAt = cabinet.CreatedAt
        };

    public static Cabinet Map(this CabinetEntity entity)
        => Cabinet.Load(
            entity.Id,
            entity.MediaUrls,
            entity.Name,
            entity.Description,
            entity.Status,
            entity.CreatedAt);
}
