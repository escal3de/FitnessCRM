using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Enums;
using FitnessCRM.Domain.Enums.SeasonTicket;

namespace FitnessCRM.Domain.Templates;

public class SeasonTicketTemplate
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public Status Status { get; private set; }
    public TicketType Type { get; private set; }
    public int ValidityPeriod { get; private set; } // ну типо выбирается единица времени, а это свойство - количество 
    public int WorkoutAmount { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private SeasonTicketTemplate(Guid id, string name, decimal price, Status status, TicketType type,
        int validityPeriod,
        int workoutAmount, DateTime? startDate, DateTime endDate, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Price = price;
        Status = status;
        Type = type;
        ValidityPeriod = validityPeriod;
        WorkoutAmount = workoutAmount;
        StartDate = startDate is not null ? startDate.Value : DateTime.MinValue;
        EndDate = endDate;
        CreatedAt = createdAt;
    }

    public static Result<SeasonTicketTemplate> Create(string name, decimal price, TicketType type,
        int validityPeriod, int workoutAmount, DateTime? startDate, DateTime endDate)
    {
        var validation = Validate(name, price, validityPeriod, workoutAmount, startDate, endDate);
        if (validation.IsFailure)
            return Result.Failure<SeasonTicketTemplate>(validation.Error);

        var seasonTicket = new SeasonTicketTemplate(Guid.NewGuid(), name, price, Status.Active, type, validityPeriod,
            workoutAmount, startDate, endDate, DateTime.UtcNow);

        return Result.Success(seasonTicket);
    }

    private static Result Validate(string name, decimal price, int validityPeriod, int workoutAmount,
        DateTime? startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Template name is required");

        if (name.Length > 200)
            return Result.Failure("Template name must not exceed 200 characters");

        if (price <= 0)
            return Result.Failure("Price must be greater than 0");

        if (validityPeriod <= 0)
            return Result.Failure("Validity period must be greater than 0");

        if (workoutAmount <= 0)
            return Result.Failure("Workout amount must be greater than 0");

        if (startDate is not null && startDate.Value < DateTime.UtcNow)
            return Result.Failure("Start date cannot be in the past");

        var effectiveStart = startDate ?? DateTime.MinValue;
        if (endDate <= effectiveStart)
            return Result.Failure("End date must be after start date");

        return Result.Success();
    }

    public static SeasonTicketTemplate Load(Guid id, string name, decimal price, Status status, TicketType type,
        int validityPeriod, int workoutAmount, DateTime? startDate, DateTime endDate, DateTime createdAt)
        => new SeasonTicketTemplate(id, name, price, status, type, validityPeriod, workoutAmount, startDate, endDate,
            createdAt);
}