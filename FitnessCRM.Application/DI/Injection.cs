using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessCRM.Application.DI;

public static class Injection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(Injection).Assembly)
            .AddClasses(clasess => clasess.Where(type => type.Name.EndsWith("Handler")))
            .AsSelf()
            .WithScopedLifetime());
        
        services.AddValidatorsFromAssembly(typeof(Injection).Assembly);

        return services;
    }
}