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
    public string QrCodeLink { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private SeasonTicket(Guid id, string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName, string qrCodeLink, DateTime createdAt)
    {
        Id = id;
        Name = name;
        ActiveAt = activeAt;
        ExpiresAt = expiresAt;
        RemainingWorkouts = remainingWorkouts;
        Price = price;
        SchoolName = schoolName;
        QrCodeLink = qrCodeLink;
        CreatedAt = createdAt;
    }

    public static Result<SeasonTicket> Create(string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName, string qrCodeLink)
    {
        return Result.Success(new SeasonTicket(Guid.NewGuid(), name, activeAt, expiresAt, remainingWorkouts, price,
            schoolName, qrCodeLink, DateTime.UtcNow));
    }

    public static SeasonTicket Load(Guid id, string name, DateTime activeAt, DateTime expiresAt, int remainingWorkouts,
        decimal price, string schoolName, string qrCodeLink, DateTime createdAt)
        => new SeasonTicket(id, name, activeAt, expiresAt, remainingWorkouts, price, schoolName, qrCodeLink, createdAt);
}