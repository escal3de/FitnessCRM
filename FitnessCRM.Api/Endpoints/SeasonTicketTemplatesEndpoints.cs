using FitnessCRM.Api.Extensions;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Handlers.SeasonTicketTemplates;
using Serilog;
using static FitnessCRM.Domain.Permissions.Permissions;

namespace FitnessCRM.Api.Endpoints;

public static class SeasonTicketTemplatesEndpoints
{
    public static IEndpointRouteBuilder MapSeasonTicketTemplatesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/season-tickets-templates");

        group.MapPost("/", async (
            CreateSeasonTicketTemplateRequest request,
            CreateSeasonTicketTemplateHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to create season ticket template");

            var result = await handler.HandleAsync(request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to create season ticket template: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Created season ticket template");
            return Results.Ok(result);
        }).RequirePermission(SeasonTicketTemplatesCreate);

        group.MapGet("/", async (
            GetAllSeasonTicketTemplatesHandler handler, 
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Get all season ticket templates");
            
            var result = await handler.HandleAsync(cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get all season ticket templates: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished get all season ticket templates");
            return Results.Ok(result.Value);
        }).RequirePermission(SeasonTicketTemplatesRead);

        group.MapGet("/{id:guid}", async (
            Guid id,
            GetSeasonTicketTemplateHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get season ticket template");

            var result = await handler.HandleAsync(id, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get season ticket template: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished get season ticket template");
            return Results.Ok(result.Value);
        }).RequirePermission(SeasonTicketTemplatesRead);

        group.MapDelete("/{id:guid}", async (
            Guid id,
            DeleteSeasonTicketTemplateHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to delete season ticket template");

            var result = await handler.HandleAsync(id, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to delete season ticket template: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished delete season ticket template");
            return Results.Ok();
        }).RequirePermission(SeasonTicketTemplatesDelete);

        return group;
    }
}