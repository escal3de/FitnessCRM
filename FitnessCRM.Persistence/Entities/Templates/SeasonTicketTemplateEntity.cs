using FitnessCRM.Domain.Enums;
using FitnessCRM.Domain.Enums.SeasonTicket;

namespace FitnessCRM.Persistence.Entities.Templates;

public class SeasonTicketTemplateEntity
{
    public Guid Id { get; set; }
    
    public Guid SchoolId { get; set; }
    public SchoolEntity School { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Status Status { get; set; }
    public TicketType Type { get; set; }
    public int ValidityPeriod { get; set; } // ну типо выбирается единица времени, а это свойство - количество 
    public int WorkoutAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}