using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain;
using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace FitnessCRM.Persistence.Realisations.Repositories;

public class UsersRepository(AppDbContext dbContext) : IUsersRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result> AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user.Map(), cancellationToken);

        return Result.Success();
    }

    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.ContactData)
            .Include(x => x.ClientProfile)
            .Include(x => x.TeacherProfile)
            .Include(x => x.StaffProfile)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<User>($"User with id {id} not found");
    }

    public async Task<Result<User>> GetByPhoneAsync(Phone phone, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Phone == phone, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<User>($"User with phone {phone.ToString()} not found");
    }

    public async Task<Result<User>> GetByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash, cancellationToken);
        
        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<User>($"User with refresh token {refreshTokenHash} not found");
    }

    public async Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.ContactData)
            .Include(x => x.ClientProfile)
            .Include(x => x.TeacherProfile)
            .Include(x => x.StaffProfile)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>($"Users list is empty");
    }

    public async Task<Result<IEnumerable<User>>> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.ClientProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.ClientProfile)
            .ThenInclude(x => x.SeasonTickets)
            .Include(x => x.ClientProfile)
            .ThenInclude(x => x.ClientSchools)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>($"Users list is empty");
    }

    public async Task<Result<IEnumerable<User>>> GetAllTeachersAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.TeacherProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.TeacherProfile)
            .ThenInclude(x => x.Directions.Select(x => x.Name))
            .Include(x => x.TeacherProfile).ThenInclude(x => x.School)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>($"Users list is empty");
    }

    public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateRefreshTokenAsync(Guid userId, string? refreshTokenHash, DateTime? refreshTokenExpiresAt,
        CancellationToken cancellationToken)
    {
        await _dbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.RefreshTokenHash, refreshTokenHash)
                .SetProperty(x => x.RefreshTokenExpiresAt, refreshTokenExpiresAt)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Users
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }
}