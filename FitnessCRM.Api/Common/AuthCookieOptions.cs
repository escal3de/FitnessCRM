namespace FitnessCRM.Api.Common;

public static class AuthCookieOptions
{
    public static CookieOptions Create(IConfiguration configuration, IHostEnvironment environment)
    {
        var secure = configuration.GetValue("CookieOptions:Secure", !environment.IsDevelopment());

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        };
    }
}