using CSharpFunctionalExtensions;
using FitnessCRM.Domain;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Application.Abstractions.Repositories;

public interface IUsersRepository
{
    Task<Result> AddAsync(User user, CancellationToken cancellationToken);
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<User>> GetByPhoneAsync(Phone phone, CancellationToken cancellationToken);
    Task<Result<User>> GetByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllClientsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllTeachersAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> GetAllStaffAsync(CancellationToken cancellationToken);
    Task<Result> UpdateAsync(User user, CancellationToken cancellationToken);

    Task<Result> UpdateRefreshTokenAsync(Guid userId, string? refreshTokenHash, DateTime? refreshTokenExpiresAt,
        CancellationToken cancellationToken);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}