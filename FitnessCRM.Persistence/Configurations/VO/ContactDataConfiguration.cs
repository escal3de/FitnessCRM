using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.VO;

public class ContactDataConfiguration : IEntityTypeConfiguration<ContactDataEntity>
{
    public void Configure(EntityTypeBuilder<ContactDataEntity> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithOne(x => x.ContactData)
            .HasForeignKey<ContactDataEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.AvatarUrl);
        
        builder.Property(x => x.Email)
            .HasConversion(
                e => e.Value, 
                t => Email.Create(t).Value)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.LastName)
            .IsRequired();

        builder.Property(x => x.Patronymic)
            .IsRequired();

        builder.Property(x => x.LastUpdatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}