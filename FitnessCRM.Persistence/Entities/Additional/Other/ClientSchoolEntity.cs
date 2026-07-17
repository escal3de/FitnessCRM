using FitnessCRM.Persistence.Entities.Additional.Profiles;

namespace FitnessCRM.Persistence.Entities.Additional.Other;

public class ClientSchoolEntity
{
    public Guid ClientProfileId { get; set; }
    public ClientProfileEntity ClientProfile { get; set; }
    
    public Guid SchoolId { get; set; }
    public SchoolEntity School { get; set; }
}
