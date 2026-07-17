using FitnessCRM.Domain.Enums;

namespace FitnessCRM.Persistence.Entities.Additional.Other;

public class CabinetEntity
{
    public Guid Id { get; set; }
    
    public Guid SchoolId { get; set; }
    public SchoolEntity School { get; set; }
    
    public List<string> MediaUrls { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}