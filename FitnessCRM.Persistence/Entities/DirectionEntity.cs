using FitnessCRM.Domain.Enums;
using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Entities;

public class DirectionEntity
{
    public Guid Id { get; set; }
    
    public Guid SchoolId { get; set; }
    public SchoolEntity School { get; set; }
    
    public Guid TeacherProfileId { get; set; }
    public TeacherProfileEntity TeacherProfile { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}