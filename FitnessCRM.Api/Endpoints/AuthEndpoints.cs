using FitnessCRM.Api.Common;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Application.Handlers.Auth.Login;
using FitnessCRM.Application.Handlers.Auth.Refresh;
using FitnessCRM.Application.Handlers.Auth.Register;
using Serilog;
using static FitnessCRM.Api.ServicesCollections.RateLimiting.Policies;

namespace FitnessCRM.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth")
            .AllowAnonymous();

        group.MapPost("/register", async (
            RegisterUserRequest request,
            RegisterUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start user registration");

            var result = await handler.HandleAsync(request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to register user: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            Log.Information("Registration successful");
            return Results.Ok();
        }).RequireRateLimiting(RegisterPolicy);

        group.MapPost("/login", async (
            LoginUserRequest request,
            HttpContext context,
            IConfiguration configuration,
            IHostEnvironment environment,
            LoginUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start user login");

            var result = await handler.HandleAsync(request, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to login: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            var cookieOptions = AuthCookieOptions.Create(configuration, environment);

            cookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(15);
            context.Response.Cookies.Append("token", result.Value.Token, cookieOptions);

            cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(7);
            context.Response.Cookies.Append("refresh", result.Value.RefreshToken, cookieOptions);

            Log.Information("Login successful");
            return Results.Ok(result.Value);
        }).RequireRateLimiting(LoginPolicy);

        group.MapPost("/refresh", async (
            IConfiguration configuration,
            IHostEnvironment environment,
            HttpContext context,
            RefreshTokenHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            Log.Debug("Start refresh token");

            var rawRefreshToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
            {
                Log.Warning("Failed to refresh token: raw refresh token is null");
                return Results.Unauthorized();
            }

            var result = await handler.HandleAsync(rawRefreshToken, cancellationToken);
            if (result.IsFailure)
            {
                Log.Warning("Failed to refresh token: {Error}", result.Error);
                return Results.BadRequest(result.Error);
            }

            var cookieOptions = AuthCookieOptions.Create(configuration, environment);

            cookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(15);
            context.Response.Cookies.Append("token", result.Value.RefreshToken, cookieOptions);

            cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(7);
            context.Response.Cookies.Append("refresh", result.Value.RefreshToken, cookieOptions);

            Log.Information("Refresh token successful");
            return Results.Ok(result.Value);
        }).RequireRateLimiting(RefreshPolicy);

        group.MapPost("/logout", (
            IConfiguration configuration,
            IHostEnvironment environment,
            HttpContext context) =>
        {
            Log.Debug("Start user logout");

            var cookieOptions = AuthCookieOptions.Create(configuration, environment);

            context.Response.Cookies.Delete("token", cookieOptions);
            context.Response.Cookies.Delete("refreshToken", cookieOptions);

            Log.Information("Logout successful");
            return Results.Unauthorized();
        }).RequireRateLimiting(LogoutPolicy);

        return group;
    }
}