using FitnessCRM.Api.Extensions;
using Serilog.Context;

namespace FitnessCRM.Api.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        Guid? userId = null;

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdResult = context.User.TryGetUserGuid();
            if (userIdResult.IsSuccess)
                userId = userIdResult.Value;
        }

        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
        using (LogContext.PushProperty("ClientIp", context.Connection.RemoteIpAddress))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"]))
        {
            await _next(context);
        }
    }
}