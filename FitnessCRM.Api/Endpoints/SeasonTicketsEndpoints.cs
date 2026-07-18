using System.Security.Claims;
using FitnessCRM.Api.Extensions;
using FitnessCRM.Application.Handlers.SeasonTickets;
using Serilog;
using static FitnessCRM.Domain.Permissions.Permissions;

namespace FitnessCRM.Api.Endpoints;

public static class SeasonTicketsEndpoints
{
    public static IEndpointRouteBuilder MapSeasonTicketsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/season-tickets");

        group.MapPost("/", async () =>
        {
            
        });

        group.MapGet("/all", async (
            GetAllSeasonTicketsHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get all season tickets");
            
            var result = await handler.HandleAsync(cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get all season tickets: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished get all season tickets");
            return Results.Ok(result);
        }).RequirePermission(UserReadAllSeasonTickets);

        group.MapGet("/for-trainer/{id:guid}", async (
            Guid id,
            GetSeasonTicketHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get season ticket: {Id}", id);
            
            var result = await handler.HandleAsync(id, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get season ticket: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished get season ticket");
            return Results.Ok(result.Value);
        }).RequirePermission(UserReadSeasonTickets);

        group.MapGet("/by-client/{clientId:guid}", async (
            Guid clientId,
            GetAllSeasonTicketsHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start get client season tickets");

            var result = await handler.HandleAsync(cancellationToken, clientId);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get client season tickets: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished get client season tickets");
            return Results.Ok(result.Value);
        }).RequirePermission(UserReadSeasonTickets);

        group.MapGet("/", async (
            ClaimsPrincipal claims,
            GetAllSeasonTicketsHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get season tickets");

            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to get season tickets: {Error}", userId);
                return Results.BadRequest(userId.Error);
            }

            var result = await handler.HandleAsync(cancellationToken: cancellationToken, userId: userId.Value);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get season tickets: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished get season tickets");
            return Results.Ok(result.Value);
        }).RequirePermission(MeReadSeasonTickets);
        
        return group;
    }
}