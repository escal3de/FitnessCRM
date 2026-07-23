using CSharpFunctionalExtensions;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Application.Abstractions.Repositories;

public interface IUsersRepository
{
    Task<Result> AddAsync(User user, CancellationToken cancellationToken);
    Task<Result<User>> GetByIdAsync(Guid id, Guid? schoolId, CancellationToken cancellationToken);
    Task<Result<User>> GetByPhoneAsync(Phone phone, CancellationToken cancellationToken);
    Task<Result<User>> GetByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken);
    Task<Result<School>> GetSchoolByIdAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllClientsBySchoolAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllTeachersBySchoolAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllStaffBySchoolAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(User user, CancellationToken cancellationToken);

    Task<Result> UpdateRefreshTokenAsync(Guid userId, string? refreshTokenHash, DateTime? refreshTokenExpiresAt,
        CancellationToken cancellationToken);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}