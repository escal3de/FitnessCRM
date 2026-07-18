using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Domain.Additional.Profiles;

public class ClientProfile
{
    public Guid Id { get; private set; }
    public int? Height { get; private set; }
    public int? Weight { get; private set; }
    public string? MedicalContraindications { get; private set; }
    public List<School> Schools { get; }
    public List<SeasonTicket> SeasonTickets { get; private set; }
    public string? ParentsCommunication { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ClientProfile(Guid id, int? height, int? weight, string? medicalContraindications, List<School> schools,
        List<SeasonTicket> seasonTickets, string? parentsCommunication, DateTime createdAt)
    {
        Id = id;
        Height = height;
        Weight = weight;
        MedicalContraindications = medicalContraindications;
        Schools = schools;
        SeasonTickets = seasonTickets;
        ParentsCommunication = parentsCommunication;
        CreatedAt = createdAt;
    }

    public static Result<ClientProfile> Create(int? height, int? weight)
    {
        var validation = Validate(height, weight);
        if (validation.IsFailure)
            return Result.Failure<ClientProfile>(validation.Error);

        return Result.Success(new ClientProfile(Guid.NewGuid(), height, weight,
            null, [], [], null, DateTime.UtcNow));
    }

    private static Result Validate(int? height, int? weight)
    {
        if (height is not null && (height < 50 || height > 300))
            return Result.Failure("Height must be between 50 and 300 cm");

        if (weight is not null && (weight < 20 || weight > 500))
            return Result.Failure("Weight must be between 20 and 500 kg");

        return Result.Success();
    }

    public static ClientProfile Load(Guid id, int? height, int? weight, string? medicalContraindications,
        List<School> schools, List<SeasonTicket> seasonTickets, string? parentsCommunication,
        DateTime createdAt)
        => new ClientProfile(id, height, weight, medicalContraindications, schools, seasonTickets,
            parentsCommunication, createdAt);
}