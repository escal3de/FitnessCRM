using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain;
using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.VO;
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
            : Result.Failure<IEnumerable<User>>("Users list is empty");
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
            : Result.Failure<IEnumerable<User>>("Users list is empty");
    }

    public async Task<Result<IEnumerable<User>>> GetAllStaffAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.TeacherProfile != null)
            .Include(x => x.ContactData)
            .Include(x => x.StaffProfile)
            .ThenInclude(x => x.Schools)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<User>>("Users list is empty");
    }

    public async Task<Result> UpdateAsync(Guid id, Phone? phone, Email? email, string? avatarUrl,
        string? name, string? lastName, string? patronymic, DateTime? birthday, string? profession,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.ContactData)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user is null)
            return Result.Failure($"User with id {id} not found");

        if (phone is not null)
            user.Phone = phone;

        if (user.ContactData is null)
        {
            user.ContactData = new ContactDataEntity
            {
                UserId = id,
                AvatarUrl = avatarUrl,
                Email = email,
                Name = name ?? string.Empty,
                LastName = lastName ?? string.Empty,
                Patronymic = patronymic ?? string.Empty,
                Birthday = birthday,
                Profession = profession,
                LastUpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }
        else
        {
            if (avatarUrl is not null)
                user.ContactData.AvatarUrl = avatarUrl;
            if (email is not null)
                user.ContactData.Email = email;
            if (name is not null)
                user.ContactData.Name = name;
            if (lastName is not null)
                user.ContactData.LastName = lastName;
            if (patronymic is not null)
                user.ContactData.Patronymic = patronymic;
            if (birthday is not null)
                user.ContactData.Birthday = birthday;
            if (profession is not null)
                user.ContactData.Profession = profession;

            user.ContactData.LastUpdatedAt = DateTime.UtcNow;
        }

        user.UpdatedAt = DateTime.UtcNow;

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