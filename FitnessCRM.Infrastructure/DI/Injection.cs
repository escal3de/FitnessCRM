using FitnessCRM.Application.Abstractions.Caching;
using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Domain;
using FitnessCRM.Infrastructure.Realisations.Caching;
using FitnessCRM.Infrastructure.Realisations.Caching.Options;
using FitnessCRM.Infrastructure.Realisations.Security;
using FitnessCRM.Infrastructure.Realisations.Security.Options;
using FitnessCRM.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessCRM.Infrastructure.DI;

public static class Injection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<CacheOptions>(configuration.GetSection(nameof(CacheOptions)));

        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}