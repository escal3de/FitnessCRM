using System.Security.Claims;
using System.Threading.RateLimiting;

namespace FitnessCRM.Api.ServicesCollections;

public static class CustomRateLimiter
{
    private static string GetPartitionKey(HttpContext context)
        => context.User.FindFirstValue("sub")
           ?? context.Connection.RemoteIpAddress?.ToString()
           ?? "anonymous";

    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
        => services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new()
                    {
                        AutoReplenishment = true,
                        PermitLimit = 1000,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(3)
                    }));

            options.OnRejected = async (context, token) =>
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status429TooManyRequests;
                response.Headers["Retry-After"] = "60";

                await response.WriteAsJsonAsync("""{"error": "Too many requests"}""", token);
            };

            // policies
            options.AddPolicy<string>("LoginPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new()
                    {
                        AutoReplenishment = true,
                        PermitLimit = 15,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            options.AddPolicy<string>("RegisterPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new()
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    }));
            
            options.AddPolicy<string>("RefreshPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(5),
                        QueueLimit = 0
                    }));
            
            options.AddPolicy<string>("LogoutPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(5),
                        QueueLimit = 0
                    }));
        });
}