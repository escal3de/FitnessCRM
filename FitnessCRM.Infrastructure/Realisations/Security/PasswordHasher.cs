using FitnessCRM.Application.Abstractions.Security;
using FitnessCRM.Domain;
using Microsoft.AspNetCore.Identity;

namespace FitnessCRM.Infrastructure.Realisations.Security;

public class PasswordHasher(IPasswordHasher<User> passwordHasher) : IPasswordHasher
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public string HashPassword(string password) => _passwordHasher.HashPassword(null!, password);

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);

        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}