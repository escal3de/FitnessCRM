using FitnessCRM.Api.Common;
using FitnessCRM.Api.Endpoints;
using FitnessCRM.Api.Middlewares;
using FitnessCRM.Api.ServicesCollections;
using FitnessCRM.Application.DI;
using FitnessCRM.Infrastructure.DI;
using FitnessCRM.Persistence.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);

builder.Services.AddCustomAuth(configuration);
builder.Services.AddCustomRateLimiter();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHsts();
app.UseRateLimiter();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUsersEndpoints();
app.MapSeasonTicketsEndpoints();

app.Run();