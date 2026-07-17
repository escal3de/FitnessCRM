using FitnessCRM.Domain;
using FitnessCRM.Persistence.Entities;
using FitnessCRM.Persistence.Mappers.Additional.Profiles;
using FitnessCRM.Persistence.Mappers.VO;

namespace FitnessCRM.Persistence.Mappers;

public static class UserMapper
{
    public static UserEntity Map(this User user)
        => new UserEntity
        {
            Id = user.Id,
            Phone = user.Phone,
            ContactData = user.ContactData?.Map(user.Id),
            ClientProfile = user.ClientProfile?.Map(user.Id),
            TeacherProfile = user.TeacherProfile?.Map(user.Id),
            StaffProfile = user.StaffProfile?.Map(user.Id),
            Role = user.Role,
            RefreshTokenHash = user.RefreshTokenHash,
            RefreshTokenExpiresAt = user.RefreshTokenExpiresAt,
            HashedPassword = user.HashedPassword,
            TokenVersion = user.TokenVersion,
            IsTwoFactorEnabled = user.IsTwoFactorEnabled,
            UpdatedAt = user.UpdateAt,
            CreatedAt = user.CreatedAt
        };

    public static User Map(this UserEntity entity)
        => User.Load(
            entity.Id,
            entity.Phone,
            entity.ContactData?.Map(),
            entity.ClientProfile?.Map(),
            entity.TeacherProfile?.Map(),
            entity.StaffProfile?.Map(),
            entity.Role,
            entity.RefreshTokenHash,
            entity.RefreshTokenExpiresAt,
            entity.HashedPassword,
            entity.TokenVersion,
            entity.IsTwoFactorEnabled,
            entity.UpdatedAt,
            entity.CreatedAt);
}