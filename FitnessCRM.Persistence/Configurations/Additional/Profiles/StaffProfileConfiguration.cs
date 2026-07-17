using FitnessCRM.Persistence.Entities.Additional.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Additional.Profiles;

public class StaffProfileConfiguration : IEntityTypeConfiguration<StaffProfileEntity>
{
    public void Configure(EntityTypeBuilder<StaffProfileEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.StaffProfile)
            .HasForeignKey<StaffProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Schools)
            .WithOne(x => x.StaffProfile)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}