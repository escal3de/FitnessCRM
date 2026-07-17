using FitnessCRM.Domain.Templates;
using FitnessCRM.Persistence.Entities.Templates;

namespace FitnessCRM.Persistence.Mappers.Templates;

public static class SeasonTicketTemplateMapper
{
    public static SeasonTicketTemplateEntity Map(this SeasonTicketTemplate template, Guid schoolId)
        => new SeasonTicketTemplateEntity
        {
            Id = template.Id,
            SchoolId = schoolId,
            Name = template.Name,
            Price = template.Price,
            Status = template.Status,
            Type = template.Type,
            ValidityPeriod = template.ValidityPeriod,
            WorkoutAmount = template.WorkoutAmount,
            StartDate = template.StartDate,
            EndDate = template.EndDate,
            CreatedAt = template.CreatedAt
        };

    public static SeasonTicketTemplate Map(this SeasonTicketTemplateEntity entity)
        => SeasonTicketTemplate.Load(
            entity.Id,
            entity.Name,
            entity.Price,
            entity.Status,
            entity.Type,
            entity.ValidityPeriod,
            entity.WorkoutAmount,
            entity.StartDate,
            entity.EndDate,
            entity.CreatedAt);
}