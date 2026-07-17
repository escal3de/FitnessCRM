using FitnessCRM.Persistence.Entities.Additional.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCRM.Persistence.Configurations.Additional.Profiles;

public class ClientProfileConfiguration : IEntityTypeConfiguration<ClientProfileEntity>
{
    public void Configure(EntityTypeBuilder<ClientProfileEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.ClientProfile)
            .HasForeignKey<ClientProfileEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Height);

        builder.Property(x => x.Weight);

        builder.Property(x => x.MedicalContraindications);
        
        builder.Property(x => x.UserGender)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(x => x.ClientSchools)
            .WithOne(x => x.ClientProfile)
            .HasForeignKey(x => x.ClientProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.SeasonTickets)
            .WithOne(x => x.ClientProfile)
            .HasForeignKey(x => x.ClientProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.ParentsCommunication);
    }
}