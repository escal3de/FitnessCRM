using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain.VO;

public sealed class Education
{
    public int Level { get; }
    public string Name { get; }
    public string Specialization { get; }
    public DateTime EndedAt { get; }

    private Education(int level, string name, string specialization, DateTime endedAt)
    {
        Level = level;
        Name = name;
        Specialization = specialization;
        EndedAt = endedAt;
    }

    public static Result<Education> Create(int level, string name, string specialization, DateTime endedAt)
    {
        var validation = Validate(name, specialization);
        if (validation.IsFailure)
            return Result.Failure<Education>(validation.Error);

        return Result.Success(new Education(level, name, specialization, endedAt));
    }

    private static Result Validate(string name, string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Education name is required");

        if (name.Length > 200)
            return Result.Failure("Education name must not exceed 200 characters");

        if (string.IsNullOrWhiteSpace(specialization))
            return Result.Failure("Specialization is required");

        if (specialization.Length > 200)
            return Result.Failure("Specialization must not exceed 200 characters");

        return Result.Success();
    }

    public static Education Load(int level, string name, string specialization, DateTime endedAt)
        => new Education(level, name, specialization, endedAt);
}