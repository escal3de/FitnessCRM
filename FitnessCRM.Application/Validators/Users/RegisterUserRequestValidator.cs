using FitnessCRM.Application.Contracts.Auth;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Номер телефона обязателен")
            .Must(x => x.StartsWith('+')).WithMessage("Номер должен начинаться с '+'")
            .Matches(@"^\+\d{10,15}$").WithMessage("Некорректный формат номера");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов")
            .MaximumLength(128).WithMessage("Пароль не должен превышать 128 символов")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");
    }
}
