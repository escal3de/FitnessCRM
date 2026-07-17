using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Entities.VO;

public class EducationEntity
{
    public Guid TeacherProfileId { get; set; }
    public TeacherProfileEntity TeacherProfile { get; set; }
    
    public int Level { get; set; }
    public string Name { get; set; }
    public string Specialization { get; set; }
    public DateTime EndedAt { get; set; }
}