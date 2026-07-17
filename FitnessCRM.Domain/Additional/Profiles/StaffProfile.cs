using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain.Additional.Profiles;

public class StaffProfile
{
    public Guid Id { get; private set; }
    public List<School>? Schools { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private StaffProfile(Guid id, List<School>? schools, DateTime createdAt)
    {
        Id = id;
        Schools = schools;
        CreatedAt = createdAt;
    }

    public static Result<StaffProfile> Create()
    {
        return Result.Success(new StaffProfile(Guid.NewGuid(), [], DateTime.UtcNow));
    }

    public static StaffProfile Load(Guid id, List<School>? schools, DateTime createdAt)
        => new StaffProfile(id, schools, createdAt);
}