using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<SchoolEntity>
{
    public void Configure(EntityTypeBuilder<SchoolEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.AvatarUrl);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.ContactEmail)
            .HasConversion(
                e => e.Value, 
                t => Email.Create(t).Value)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(x => x.Cabinets)
            .WithOne(x => x.School)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Directions)
            .WithOne(x => x.School)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.SeasonTicketTemplates)
            .WithOne(x => x.School)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}