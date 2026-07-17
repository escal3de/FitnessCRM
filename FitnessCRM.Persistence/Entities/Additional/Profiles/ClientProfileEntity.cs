using FitnessCRM.Domain.Enums.User;
using FitnessCRM.Persistence.Entities.Additional.Other;

namespace FitnessCRM.Persistence.Entities.Additional.Profiles;

public class ClientProfileEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public int? Height { get; set; }
    public int? Weight { get; set; }
    public string? MedicalContraindications { get; set; }
    public UserGender UserGender { get; set; }
    
    public List<ClientSchoolEntity> ClientSchools { get; set; }
    
    public List<SeasonTicketEntity> SeasonTickets { get; set; }
    public string? ParentsCommunication { get; set; }
    public DateTime CreatedAt { get; set; }
}