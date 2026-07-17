using FitnessCRM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations;

public class DirectionConfiguration : IEntityTypeConfiguration<DirectionEntity>
{
    public void Configure(EntityTypeBuilder<DirectionEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.School)
            .WithMany(x => x.Directions)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.CreatedAt);
    }
}