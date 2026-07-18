using CSharpFunctionalExtensions;
using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Domain;
using FitnessCRM.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace FitnessCRM.Persistence.Realisations.Repositories;

public class SeasonTicketRepository(AppDbContext dbContext) : ISeasonTicketRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Result> AddAsync(SeasonTicket ticket, Guid? clientId, CancellationToken cancellationToken)
    {
        await _dbContext.SeasonTickets.AddAsync(ticket.Map(clientId), cancellationToken);

        return Result.Success();
    }

    public async Task<Result<SeasonTicket>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _dbContext.SeasonTickets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result is not null
            ? Result.Success(result.Map())
            : Result.Failure<SeasonTicket>($"Season ticket with id {id} not found");
    }

    public async Task<Result<IEnumerable<SeasonTicket>>> GetAllByClientProfileIdAsync(Guid clientProfileId,
        CancellationToken cancellationToken)
    {
        var result = await _dbContext.SeasonTickets
            .AsNoTracking()
            .Where(x => x.ClientProfileId == clientProfileId)
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<SeasonTicket>>($"Season tickets for client profile id {clientProfileId} not found");
    }

    public async Task<Result<IEnumerable<SeasonTicket>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.SeasonTickets
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result.Count != 0
            ? Result.Success(result.Select(x => x.Map()))
            : Result.Failure<IEnumerable<SeasonTicket>>("Season tickets not found");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.SeasonTickets
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return Result.Success();
    }
}