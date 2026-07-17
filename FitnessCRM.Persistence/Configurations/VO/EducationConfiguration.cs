using FitnessCRM.Persistence.Entities.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.VO;

public class EducationConfiguration : IEntityTypeConfiguration<EducationEntity>
{
    public void Configure(EntityTypeBuilder<EducationEntity> builder)
    {
        builder.HasOne(x => x.TeacherProfile)
            .WithOne(x => x.Education)
            .HasForeignKey<EducationEntity>(x => x.TeacherProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.Level)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Specialization)
            .IsRequired();
        
        builder.Property(x => x.EndedAt)
            .IsRequired();
    }
}