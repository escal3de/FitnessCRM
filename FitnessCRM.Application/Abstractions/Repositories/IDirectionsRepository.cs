using CSharpFunctionalExtensions;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Additional.Profiles;

namespace FitnessCRM.Application.Abstractions.Repositories;

public interface IDirectionsRepository
{
    Task<Result> AddAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken);
    Task<Result<Direction>> GetByIdAsync(Guid id, Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Direction>>> GetAllAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Direction direction, Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<TeacherProfile>>> GetTeacherProfilesByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
}