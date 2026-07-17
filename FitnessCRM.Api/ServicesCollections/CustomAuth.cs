using System.Text;
using FitnessCRM.Infrastructure.Realisations.Security.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FitnessCRM.Api.ServicesCollections;

public static class CustomAuth
{
    public static IServiceCollection AddCustomAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetRequiredSection("JwtOptions"));

        var jwtOptions = configuration.GetRequiredSection("JwtOptions").Get<JwtOptions>()
                         ?? throw new InvalidOperationException($"Missing {nameof(JwtOptions)}");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["token"];

                        if (!string.IsNullOrWhiteSpace(accessToken))
                            context.Token = accessToken;
                        
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}