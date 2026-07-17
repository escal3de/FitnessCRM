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
        return Result.Success(new Education(level, name, specialization, endedAt));
    }

    public static Education Load(int level, string name, string specialization, DateTime endedAt)
        => new Education(level, name, specialization, endedAt);
}