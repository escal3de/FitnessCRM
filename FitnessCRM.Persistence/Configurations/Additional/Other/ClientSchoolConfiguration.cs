using FitnessCRM.Persistence.Entities.Additional.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Additional.Other;

public class ClientSchoolConfiguration : IEntityTypeConfiguration<ClientSchoolEntity>
{
    public void Configure(EntityTypeBuilder<ClientSchoolEntity> builder)
    {
        builder.HasKey(x => new { x.ClientProfileId, x.SchoolId });

        builder.HasOne(x => x.ClientProfile)
            .WithMany(x => x.ClientSchools)
            .HasForeignKey(x => x.ClientProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.School)
            .WithMany(x => x.ClientSchools)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
