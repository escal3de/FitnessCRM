using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.VO;
using FitnessCRM.Persistence.Mappers;
using FitnessCRM.Persistence.Mappers.VO;
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

    public async Task<Result<User>> GetByIdAsync(Guid id, Guid? schoolId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Include(x => x.ContactData)
            .Include(x => x.ClientProfile)
            .Include(x => x.TeacherProfile)
            .Include(x => x.StaffProfile)
            .AsQueryable();

        if (schoolId.HasValue)
            query = query.Where(x => x.SchoolId == schoolId.Value);

        var result = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

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

    public async Task<Result<School>> GetSchoolByIdAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Schools
            .AsNoTracking()
            .Include(x => x.Cabinets)
            .Include(x => x.Directions)
            .Include(x => x.SeasonTicketTemplates)
            .FirstOrDefaultAsync(x => x.Id == schoolId, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<School>($"School with id {schoolId} not found");
    }

    public async Task<Result<IEnumerable<User>>> GetAllClientsBySchoolAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Where(x => x.ClientProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.ClientProfile)
            .ThenInclude(x => x.SeasonTickets)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>("Users list is empty");
    }

    public async Task<Result<IEnumerable<User>>> GetAllTeachersBySchoolAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Where(x => x.TeacherProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.TeacherProfile)
            .ThenInclude(x => x.Directions.Select(x => x.Name))
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>("Users list is empty");
    }

    public async Task<Result<IEnumerable<User>>> GetAllStaffBySchoolAsync(Guid schoolId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Where(x => x.StaffProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.StaffProfile)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>("Users list is empty");
    }

    public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users
            .Include(x => x.ContactData)
            .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

        if (entity is null)
            return Result.Failure($"User with id {user.Id} not found");

        entity.Phone = user.Phone;
        entity.UpdatedAt = DateTime.UtcNow;

        if (user.ContactData is not null)
        {
            if (entity.ContactData is null)
            {
                entity.ContactData = user.ContactData.Map(user.Id);
            }
            else
            {
                entity.ContactData.AvatarUrl = user.ContactData.AvatarUrl;
                entity.ContactData.Email = user.ContactData.Email;
                entity.ContactData.Name = user.ContactData.Name;
                entity.ContactData.LastName = user.ContactData.LastName;
                entity.ContactData.Patronymic = user.ContactData.Patronymic;
                entity.ContactData.Birthday = user.ContactData.Birthday;
                entity.ContactData.Profession = user.ContactData.Profession;
                entity.ContactData.LastUpdatedAt = DateTime.UtcNow;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
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
