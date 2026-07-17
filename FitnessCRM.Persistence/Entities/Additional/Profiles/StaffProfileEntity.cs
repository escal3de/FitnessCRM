namespace FitnessCRM.Persistence.Entities.Additional.Profiles;

public class StaffProfileEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public List<SchoolEntity>? Schools { get; set; }
    public DateTime CreatedAt { get; set; }
}