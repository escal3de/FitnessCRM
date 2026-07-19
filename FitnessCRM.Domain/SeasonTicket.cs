using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain;

public class SeasonTicket
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime ActiveAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public int RemainingWorkouts { get; private set; }
    public decimal Price { get; private set; }
    public string SchoolName { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private SeasonTicket(Guid id, string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName, DateTime createdAt)
    {
        Id = id;
        Name = name;
        ActiveAt = activeAt;
        ExpiresAt = expiresAt;
        RemainingWorkouts = remainingWorkouts;
        Price = price;
        SchoolName = schoolName;
        CreatedAt = createdAt;
    }

    public static Result<SeasonTicket> Create(string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName)
    {
        var validation = Validate(name, activeAt, expiresAt, remainingWorkouts, price, schoolName);
        if (validation.IsFailure)
            return Result.Failure<SeasonTicket>(validation.Error);

        return Result.Success(new SeasonTicket(Guid.NewGuid(), name, activeAt, expiresAt, remainingWorkouts, price,
            schoolName, DateTime.UtcNow));
    }

    private static Result Validate(string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Season ticket name is required");

        if (name.Length > 200)
            return Result.Failure("Season ticket name must not exceed 200 characters");

        if (expiresAt <= activeAt)
            return Result.Failure("Expiration date must be after activation date");

        if (remainingWorkouts < 0)
            return Result.Failure("Remaining workouts cannot be negative");

        if (price < 0)
            return Result.Failure("Price cannot be negative");

        if (string.IsNullOrWhiteSpace(schoolName))
            return Result.Failure("School name is required");

        if (schoolName.Length > 200)
            return Result.Failure("School name must not exceed 200 characters");

        return Result.Success();
    }

    public static SeasonTicket Load(Guid id, string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName, DateTime createdAt)
        => new SeasonTicket(id, name, activeAt, expiresAt, remainingWorkouts, price, schoolName, createdAt);
}