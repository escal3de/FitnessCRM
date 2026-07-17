using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain.VO;

public sealed class Email
{
    public string Value { get; }
    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        var validationResult = Validate(email);
        if (validationResult.IsFailure)
            return Result.Failure<Email>(validationResult.Error);
        
        return Result.Success(new Email(email));
    }

    private static Result Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure("Email cannot be empty");

        if (!(email.Contains('@') && email.Contains('.')))
            return Result.Failure("Invalid email");
        
        return Result.Success();
    }
}