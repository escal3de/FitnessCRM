using FitnessCRM.Persistence.Entities;
using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Entities.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Phone, p =>
        {
            p.Property(x => x.Value);
            p.Property(x => x.LanguageCode);
        });

        builder.HasOne(x => x.ContactData)
            .WithOne(x => x.User)
            .HasForeignKey<ContactDataEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ClientProfile)
            .WithOne(x => x.User)
            .HasForeignKey<ClientProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.TeacherProfile)
            .WithOne(x => x.User)
            .HasForeignKey<TeacherProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.StaffProfile)
            .WithOne(x => x.User)
            .HasForeignKey<StaffProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.RefreshTokenHash);

        builder.Property(x => x.RefreshTokenExpiresAt);

        builder.Property(x => x.HashedPassword)
            .IsRequired();

        builder.Property(x => x.TokenVersion)
            .IsRequired();

        builder.Property(x => x.IsTwoFactorEnabled)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}