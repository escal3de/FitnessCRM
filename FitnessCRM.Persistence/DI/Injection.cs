using FitnessCRM.Application.Abstractions.Repositories;
using FitnessCRM.Persistence.Realisations.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessCRM.Persistence.DI;

public static class Injection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}