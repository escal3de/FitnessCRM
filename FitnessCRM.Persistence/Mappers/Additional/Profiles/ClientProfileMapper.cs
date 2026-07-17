using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Mappers.Additional.Profiles;

public static class ClientProfileMapper
{
    public static ClientProfileEntity Map(this ClientProfile profile, Guid userId)
        => new ClientProfileEntity
        {
            Id = profile.Id,
            UserId = userId,
            Height = profile.Height,
            Weight = profile.Weight,
            MedicalContraindications = profile.MedicalContraindications,
            UserGender = profile.UserGender,
            SeasonTickets = profile.SeasonTickets.Select(x => x.Map()).ToList(),
            ParentsCommunication = profile.ParentsCommunication,
            CreatedAt = profile.CreatedAt
        };

    public static ClientProfile Map(this ClientProfileEntity entity)
        => ClientProfile.Load(
            entity.Id,
            entity.Height,
            entity.Weight,
            entity.MedicalContraindications,
            entity.UserGender,
            entity.ClientSchools?.Select(x => x.School?.Map()).Where(x => x != null).ToList() ?? [],
            entity.SeasonTickets?.Select(x => x.Map()).ToList() ?? [],
            entity.ParentsCommunication,
            entity.CreatedAt);
}