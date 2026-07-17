using FitnessCRM.Persistence.Entities.Additional.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Additional.Other;

public class CabinetConfiguration : IEntityTypeConfiguration<CabinetEntity>
{
    public void Configure(EntityTypeBuilder<CabinetEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.School)
            .WithMany(x => x.Cabinets)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MediaUrls);

        builder.Property(x => x.Name)
            .IsRequired();
        
        builder.Property(x => x.Description);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}