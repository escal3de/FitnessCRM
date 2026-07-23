using FitnessCRM.Persistence.Entities;
using FitnessCRM.Persistence.Entities.Additional.Other;
using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Entities.Templates;
using FitnessCRM.Persistence.Entities.VO;
using Microsoft.EntityFrameworkCore;

namespace FitnessCRM.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SeasonTicketEntity> SeasonTickets { get; set; }
    public DbSet<SchoolEntity> Schools { get; set; }
    public DbSet<DirectionEntity> Directions { get; set; }
    public DbSet<EducationEntity> Educations { get; set; }
    public DbSet<ContactDataEntity> ContactDatas { get; set; }
    public DbSet<SeasonTicketTemplateEntity> SeasonTicketTemplates { get; set; }
    public DbSet<ClientProfileEntity> ClientProfiles { get; set; }
    public DbSet<StaffProfileEntity> StaffProfiles { get; set; }
    public DbSet<TeacherProfileEntity> TeacherProfiles { get; set; }
    public DbSet<CabinetEntity> Cabinets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}