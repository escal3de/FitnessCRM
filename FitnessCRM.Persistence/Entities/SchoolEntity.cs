using FitnessCRM.Domain.Enums;
using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.Additional.Other;
using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Entities.Templates;

namespace FitnessCRM.Persistence.Entities;

public class SchoolEntity
{
    public Guid Id { get; set; }

    public Guid StaffId { get; set; }
    public StaffProfileEntity StaffProfile { get; set; }

    public string? AvatarUrl { get; set; }
    public string Name { get; set; }
    public Email ContactEmail { get; set; }
    public Status Status { get; set; }
    public List<CabinetEntity> Cabinets { get; set; }
    public List<DirectionEntity> Directions { get; set; }
    public List<SeasonTicketTemplateEntity> SeasonTicketTemplates { get; set; }
    public List<ClientSchoolEntity> ClientSchools { get; set; }
    public List<TeacherProfileEntity> TeacherProfiles { get; set; }
    public DateTime CreatedAt { get; set; }
}