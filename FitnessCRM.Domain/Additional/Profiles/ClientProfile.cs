using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Domain.Additional.Profiles;

public class ClientProfile
{
    public Guid Id { get; private set; }
    public int? Height { get; private set; }
    public int? Weight { get; private set; }
    public string? MedicalContraindications { get; private set; }
    public UserGender UserGender { get; private set; }
    public List<School> Schools { get; }
    public List<SeasonTicket> SeasonTickets { get; private set; }
    public string? ParentsCommunication { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ClientProfile(Guid id, int? height, int? weight, string? medicalContraindications, UserGender userGender,
        List<School> schools, List<SeasonTicket> seasonTickets, string? parentsCommunication, DateTime createdAt)
    {
        Id = id;
        Height = height;
        Weight = weight;
        MedicalContraindications = medicalContraindications;
        UserGender = userGender;
        Schools = schools;
        SeasonTickets = seasonTickets;
        ParentsCommunication = parentsCommunication;
        CreatedAt = createdAt;
    }

    public static Result<ClientProfile> Create(int? height, int? weight, UserGender userGender)
    {
        return Result.Success(new ClientProfile(Guid.NewGuid(), height, weight,
            null, userGender, [], [], null, DateTime.UtcNow));
    }

    public static ClientProfile Load(Guid id, int? height, int? weight, string? medicalContraindications,
        UserGender userGender, List<School> schools, List<SeasonTicket> seasonTickets, string? parentsCommunication,
        DateTime createdAt)
        => new ClientProfile(id, height, weight, medicalContraindications, userGender, schools, seasonTickets,
            parentsCommunication, createdAt);
}