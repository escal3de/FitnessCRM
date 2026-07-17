using System.Security.Claims;
using FitnessCRM.Api.Extensions;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Application.Handlers.Users;
using Serilog;
using static FitnessCRM.Domain.Permissions.Permissions;

namespace FitnessCRM.Api.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users");

        group.MapGet("/me", async (
            ClaimsPrincipal claims,
            GetUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to get user: {Error}", userId.Error);
                return Results.BadRequest(userId.Error);
            }

            Log.Debug("Start get me");

            var result = await handler.HandleAsync(id: userId.Value, cancellationToken: cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished get me");
            return Results.Ok(result.Value);
        }).RequirePermission(MeRead);

        group.MapGet("/{id:guid}", async (
            Guid id,
            ClaimsPrincipal claims,
            GetUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get user: {Id}", id);

            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to get user: {Error}", userId.Error);
                return Results.BadRequest(userId.Error);
            }

            var result = await handler.HandleAsync(id, cancellationToken, userId.Value);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished to get user");
            return Results.Ok(result.Value);
        }).RequirePermission(UsersRead);

        group.MapGet("/all", async (
            string? target,
            GetAllUsersHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to get users. Target: {target}", target);

            if (target is null)
            {
                Log.Warning("Failed to get users: Target cannot be empty");
                return Results.BadRequest(target);
            }

            var result = await handler.HandleAsync(target!, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to get users: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished get users: {Success}", result.Value);
            return Results.Ok(result.Value);
        }).RequirePermission(UsersRead);

        group.MapPatch("/", async (
            ClaimsPrincipal claims,
            UpdateUserRequest request,
            UpdateUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to update user");

            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to update user: {Error}", userId.Error);
                return Results.BadRequest(userId.Error);
            }

            var result = await handler.HandleAsync(userId.Value, request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to update user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished to update user");
            return Results.Ok(result);
        }).RequirePermission(MeUpdate);
        
        group.MapPatch("/{id:guid}", async (
            Guid id,
            ClaimsPrincipal claims,
            UpdateUserRequest request,
            UpdateUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start to update user");

            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to update user: {Error}", userId.Error);
                return Results.BadRequest(userId.Error);
            }

            var result = await handler.HandleAsync(id, request, cancellationToken, userId.Value);
            if (result.IsFailure)
            {
                Log.Warning("Failed to update user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }
            
            Log.Information("Finished to update user");
            return Results.Ok(result);
        }).RequirePermission(UsersUpdate);

        group.MapDelete("/{id:guid}", async (
            Guid id,
            ClaimsPrincipal claims,
            DeleteUserHandler handler,
            CancellationToken cancellationToken = default
        ) =>
        {
            Log.Debug("Start to delete user: {Id}", id);

            var userId = claims.TryGetUserGuid();
            if (userId.IsFailure)
            {
                Log.Warning("Failed to get user: {Error}", userId.Error);
                return Results.BadRequest(userId.Error);
            }

            var result = await handler.HandleAsync(id, userId.Value, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to delete user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Finished to delete user");
            return Results.Ok();
        }).RequirePermission(UsersDelete);

        return group;
    }
}