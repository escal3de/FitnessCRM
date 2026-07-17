using FitnessCRM.Domain.Enums.Teacher;
using FitnessCRM.Persistence.Entities.VO;

namespace FitnessCRM.Persistence.Entities.Additional.Profiles;

public class TeacherProfileEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public PositionStatus PositionStatus { get; set; }
    public List<DirectionEntity> Directions { get; set; }
    
    public Guid SchoolId { get; set; }
    public SchoolEntity School { get; set; }
    
    public EducationEntity? Education { get; set; }
    public string? Achievements { get; set; }
}