using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.VO;

namespace FitnessCRM.Persistence.Mappers.VO;

public static class ContactDataMapper
{
    public static ContactDataEntity Map(this ContactData contactData, Guid userId)
        => new ContactDataEntity
        {
            UserId = userId,
            AvatarUrl = contactData.AvatarUrl,
            Email = contactData.Email,
            Name = contactData.Name,
            LastName = contactData.LastName,
            Patronymic = contactData.Patronymic,
            Birthday = contactData.Birthday,
            UserGender = contactData.UserGender,
            Profession = contactData.Profession,
            LastUpdatedAt = contactData.LastUpdatedAt,
            CreatedAt = contactData.CreatedAt
        };

    public static ContactData Map(this ContactDataEntity entity)
        => ContactData.Load(
            entity.AvatarUrl,
            entity.Email,
            entity.Name,
            entity.LastName,
            entity.Patronymic,
            entity.Birthday,
            entity.UserGender,
            entity.Profession,
            entity.LastUpdatedAt,
            entity.CreatedAt);
}
