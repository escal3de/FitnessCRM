using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Entities.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Additional.Profiles;

public class TeacherProfileConfiguration : IEntityTypeConfiguration<TeacherProfileEntity>
{
    public void Configure(EntityTypeBuilder<TeacherProfileEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.TeacherProfile)
            .HasForeignKey<TeacherProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.PositionStatus)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(x => x.Directions)
            .WithMany(x => x.TeacherProfiles);

        builder.HasOne(x => x.Education)
            .WithOne(x => x.TeacherProfile)
            .HasForeignKey<EducationEntity>(x => x.TeacherProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Achievements);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}