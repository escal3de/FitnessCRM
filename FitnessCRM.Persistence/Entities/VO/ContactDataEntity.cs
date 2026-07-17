using FitnessCRM.Domain.VO;

namespace FitnessCRM.Persistence.Entities.VO;

public class ContactDataEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    
    public string? AvatarUrl { get; set; }
    public Email? Email { get; set; }
    
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; } // отчество лол хвахвха
    public DateTime? Birthday { get; set; }
    public string? Profession { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}