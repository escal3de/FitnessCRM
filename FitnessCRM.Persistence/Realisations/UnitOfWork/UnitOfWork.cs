using FitnessCRM.Application.Abstractions.UnitOfWork;

namespace FitnessCRM.Persistence.Realisations.UnitOfWork;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}