using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Templates;

namespace FitnessCRM.Application.Abstractions.Repositories;

public interface ISeasonTicketTemplatesRepository
{
    Task<Result> AddAsync(SeasonTicketTemplate seasonTicketTemplate, Guid schoolId, CancellationToken cancellationToken);
    Task<Result<SeasonTicketTemplate>> GetByIdAsync(Guid id, Guid schoolId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<SeasonTicketTemplate>>> GetAllBySchoolAsync(Guid schoolId, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(SeasonTicketTemplate seasonTicketTemplate, Guid schoolId, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid id, Guid schoolId, CancellationToken cancellationToken);
}