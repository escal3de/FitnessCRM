using FitnessCRM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations;

public class SeasonTicketConfiguration : IEntityTypeConfiguration<SeasonTicketEntity>
{
    public void Configure(EntityTypeBuilder<SeasonTicketEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.ClientProfile)
            .WithMany(x => x.SeasonTickets)
            .HasForeignKey(x => x.ClientProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.ActiveAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.RemainingWorkouts)
            .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired();

        builder.Property(x => x.SchoolName)
            .IsRequired();
        
        builder.Property(x => x.QrCodeLink)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}