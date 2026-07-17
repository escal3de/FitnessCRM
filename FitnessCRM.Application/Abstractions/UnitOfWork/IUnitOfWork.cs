namespace FitnessCRM.Application.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}