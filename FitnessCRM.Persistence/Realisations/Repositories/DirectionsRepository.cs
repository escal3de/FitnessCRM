using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Persistence.Mappers;
using FitnessCRM.Persistence.Mappers.Additional.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FitnessCRM.Persistence.Realisations.Repositories;

public class DirectionsRepository(AppDbContext dbContext) : IDirectionsRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<Result> AddAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken)
    {
        var entity = direction.Map(schoolId);
        await _dbContext.Directions.AddAsync(entity, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<Direction>> GetByIdAsync(Guid id, Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Directions
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Include(x => x.TeacherProfiles)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<Direction>($"Direction with id {id} not found");
    }

    public async Task<Result<IEnumerable<Direction>>> GetAllAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Directions
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Include(x => x.TeacherProfiles)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<Direction>>("Directions not found");
    }

    public async Task<Result> UpdateAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Directions
            .Include(x => x.TeacherProfiles)
            .FirstOrDefaultAsync(x => x.Id == direction.Id && x.SchoolId == schoolId, cancellationToken);

        if (entity is null)
            return Result.Failure($"Direction with id {direction.Id} not found");

        entity.Name = direction.Name;
        entity.Description = direction.Description;
        entity.Status = direction.Status;

        entity.TeacherProfiles.Clear();
        foreach (var teacherId in direction.TeacherProfiles.Select(x => x.Id))
        {
            var teacher = await _dbContext.TeacherProfiles.FindAsync(teacherId);
            if (teacher is not null)
                entity.TeacherProfiles.Add(teacher);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken)
    {
        await _dbContext.Directions
            .Where(x => x.Id == direction.Id && x.SchoolId == schoolId)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<TeacherProfile>>> GetTeacherProfilesByIdsAsync(List<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
            return Result.Success(Enumerable.Empty<TeacherProfile>());

        var result = await _dbContext.TeacherProfiles
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return Result.Success(result.Select(x => x.Map()));
    }
}
