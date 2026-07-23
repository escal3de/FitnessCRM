using CSharpFunctionalExtensions;
using FitnessCRM.Domain;

namespace FitnessCRM.Application.Abstractions.Repositories;

public interface ISeasonTicketRepository
{
    Task<Result> AddAsync(SeasonTicket ticket, Guid? clientId, CancellationToken cancellationToken);
    Task<Result<SeasonTicket>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<SeasonTicket>>> GetAllByClientProfileIdAsync(Guid clientProfileId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<SeasonTicket>>> GetAllBySchoolIdAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<SeasonTicket>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}