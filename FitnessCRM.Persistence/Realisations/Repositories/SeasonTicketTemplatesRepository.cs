using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain.Templates;
using FitnessCRM.Persistence.Mappers.Templates;
using Microsoft.EntityFrameworkCore;

namespace FitnessCRM.Persistence.Realisations.Repositories;

public class SeasonTicketTemplatesRepository(AppDbContext dbContext) : ISeasonTicketTemplatesRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<Result> AddAsync(SeasonTicketTemplate seasonTicketTemplate, Guid schoolId, CancellationToken cancellationToken)
    {
        await _dbContext.SeasonTicketTemplates.AddAsync(seasonTicketTemplate.Map(schoolId), cancellationToken);

        return Result.Success();
    }

    public async Task<Result<SeasonTicketTemplate>> GetByIdAsync(Guid id, Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.SeasonTicketTemplates
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<SeasonTicketTemplate>("Season ticket template not found");
    }

    public async Task<Result<IEnumerable<SeasonTicketTemplate>>> GetAllBySchoolAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.SeasonTicketTemplates
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<SeasonTicketTemplate>>("Season ticket template list is empty");
    }

    public async Task<Result> UpdateAsync(SeasonTicketTemplate seasonTicketTemplate, Guid schoolId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.SeasonTicketTemplates
            .FirstOrDefaultAsync(x => x.Id == seasonTicketTemplate.Id && x.SchoolId == schoolId, cancellationToken);

        if (entity is null)
            return Result.Failure("Season ticket template not found");

        entity.Name = seasonTicketTemplate.Name;
        entity.Price = seasonTicketTemplate.Price;
        entity.Status = seasonTicketTemplate.Status;
        entity.Type = seasonTicketTemplate.Type;
        entity.ValidityPeriod = seasonTicketTemplate.ValidityPeriod;
        entity.WorkoutAmount = seasonTicketTemplate.WorkoutAmount;
        entity.StartDate = seasonTicketTemplate.StartDate;
        entity.EndDate = seasonTicketTemplate.EndDate;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, Guid schoolId, CancellationToken cancellationToken)
    {
        await _dbContext.SeasonTicketTemplates
            .Where(x => x.SchoolId == schoolId)
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return Result.Success();
    }
}