using FitnessCRM.Domain;
using FitnessCRM.Persistence.Entities;
using FitnessCRM.Persistence.Mappers.Additional.Other;
using FitnessCRM.Persistence.Mappers.Templates;

namespace FitnessCRM.Persistence.Mappers;

public static class SchoolMapper
{
    public static SchoolEntity Map(this School school, Guid? staffId = null)
        => new SchoolEntity
        {
            Id = school.Id,
            StaffId = staffId ?? Guid.Empty,
            AvatarUrl = school.AvatarUrl,
            Name = school.Name,
            ContactEmail = school.ContactEmail,
            Status = school.Status,
            Cabinets = school.Cabinets.Select(x => x.Map(school.Id)).ToList(),
            Directions = school.Directions.Select(x => x.Map(school.Id)).ToList(),
            SeasonTicketTemplates = school.SeasonTicketTemplates.Select(x => x.Map(school.Id)).ToList(),
            CreatedAt = school.CreatedAt
        };

    public static School Map(this SchoolEntity entity)
        => School.Load(
            entity.Id,
            entity.AvatarUrl,
            entity.Name,
            entity.ContactEmail,
            entity.Status,
            entity.Cabinets.Select(x => x.Map()).ToList(),
            entity.Directions.Select(x => x.Map()).ToList(),
            entity.SeasonTicketTemplates.Select(x => x.Map()).ToList(),
            entity.CreatedAt);
}
