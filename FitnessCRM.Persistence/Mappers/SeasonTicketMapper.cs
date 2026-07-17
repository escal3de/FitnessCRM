using FitnessCRM.Domain;
using FitnessCRM.Persistence.Entities;

namespace FitnessCRM.Persistence.Mappers;

public static class SeasonTicketMapper
{
    public static SeasonTicketEntity Map(this SeasonTicket seasonTicket, Guid? clientProfileId = null)
        => new SeasonTicketEntity
        {
            Id = seasonTicket.Id,
            ClientProfileId = clientProfileId ?? Guid.Empty,
            Name = seasonTicket.Name,
            ActiveAt = seasonTicket.ActiveAt,
            ExpiresAt = seasonTicket.ExpiresAt,
            RemainingWorkouts = seasonTicket.RemainingWorkouts,
            Price = seasonTicket.Price,
            SchoolName = seasonTicket.SchoolName,
            QrCodeLink = seasonTicket.QrCodeLink,
            CreatedAt = seasonTicket.CreatedAt
        };

    public static SeasonTicket Map(this SeasonTicketEntity seasonTicketEntity)
        => SeasonTicket.Load(
            seasonTicketEntity.Id,
            seasonTicketEntity.Name,
            seasonTicketEntity.ActiveAt,
            seasonTicketEntity.ExpiresAt,
            seasonTicketEntity.RemainingWorkouts,
            seasonTicketEntity.Price,
            seasonTicketEntity.SchoolName,
            seasonTicketEntity.QrCodeLink,
            seasonTicketEntity.CreatedAt);
}