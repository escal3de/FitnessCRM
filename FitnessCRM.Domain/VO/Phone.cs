using CSharpFunctionalExtensions;

namespace FitnessCRM.Domain.VO;

public sealed class Phone
{
    public string Value { get; }
    public string LanguageCode { get; }
    
    public override string ToString() => Value;

    private Phone(string value, string languageCode)
    {
        Value = value;
        LanguageCode = languageCode;
    }

    public static Result<Phone> Create(string value, string languageCode = "ru")
    {
        languageCode = languageCode.ToLowerInvariant();
        
        var validationResult = Validate(value, languageCode);
        if (validationResult.IsFailure)
            return Result.Failure<Phone>(validationResult.Error);
        
        return Result.Success(new Phone(value, languageCode));
    }

    private static Result Validate(string value, string languageCode)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure("Value cannot be empty");
        
        if (string.IsNullOrWhiteSpace(languageCode))
            return Result.Failure("Language code cannot be empty");

        if (value[0] != '+')
            return Result.Failure("Invalid format: missing '+'");

        if (languageCode.Length != 2)
            return Result.Failure("Invalid format: language code must be 2 symbols");

        if (value.Length != 11 && languageCode is "ru")
            return Result.Failure("Invalid format: if the number is Russian, it must be 11 characters long");
        
        return Result.Success();
    }
}