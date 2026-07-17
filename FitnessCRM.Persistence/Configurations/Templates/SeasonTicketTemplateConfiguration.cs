using FitnessCRM.Persistence.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Templates;

public class SeasonTicketTemplateConfiguration : IEntityTypeConfiguration<SeasonTicketTemplateEntity>
{
    public void Configure(EntityTypeBuilder<SeasonTicketTemplateEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.School)
            .WithMany(x => x.SeasonTicketTemplates)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.ValidityPeriod)
            .IsRequired();

        builder.Property(x => x.WorkoutAmount)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();
        
        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}