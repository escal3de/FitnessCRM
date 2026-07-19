using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Entities;

public class SeasonTicketEntity
{
    public Guid Id { get; set; }
    
    public Guid ClientProfileId { get; set; }
    public ClientProfileEntity ClientProfile { get; set; }
    
    public string Name { get; set; }
    public DateTime ActiveAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int RemainingWorkouts { get; set; }
    public decimal Price { get; set; }
    public string SchoolName { get; set; }
    public DateTime CreatedAt { get; set; }
}