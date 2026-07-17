namespace FitnessCRM.Infrastructure.Realisations.Security.Options;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; } = 15;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}