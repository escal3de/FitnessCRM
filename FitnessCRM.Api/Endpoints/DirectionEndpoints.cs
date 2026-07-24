using FitnessCRM.Api.Extensions;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Handlers.Directions;
using Serilog;
using static FitnessCRM.Domain.Permissions.Permissions;

namespace FitnessCRM.Api.Endpoints;

public static class DirectionEndpoints
{
    public static IEndpointRouteBuilder MapDirectionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/directions");

        group.MapPost("/", async (
            CreateDirectionRequest request,
            CreateDirectionHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to create direction");

            var result = await handler.HandleAsync(request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to create direction: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Created direction");
            return Results.Ok(result);
        }).RequirePermission(DirectionsCreate);

        group.MapGet("/", async (
            GetAllDirectionsHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get all directions");

            var result = await handler.HandleAsync(cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get all directions: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished get all directions");
            return Results.Ok(result.Value);
        }).RequirePermission(DirectionsRead);

        group.MapGet("/{id:guid}", async (
            Guid id,
            GetDirectionHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get direction: {Id}", id);

            var result = await handler.HandleAsync(id, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get direction: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished get direction");
            return Results.Ok(result.Value);
        }).RequirePermission(DirectionsRead);

        group.MapPatch("/{id:guid}", async (
            Guid id,
            UpdateDirectionRequest request,
            UpdateDirectionHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to update direction: {Id}", id);

            var result = await handler.HandleAsync(id, request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to update direction: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished update direction");
            return Results.Ok();
        }).RequirePermission(DirectionsUpdate);

        group.MapDelete("/{id:guid}", async (
            Guid id,
            DeleteDirectionHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to delete direction: {Id}", id);

            var result = await handler.HandleAsync(id, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to delete direction: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished delete direction");
            return Results.Ok();
        }).RequirePermission(DirectionsDelete);

        return group;
    }
}
