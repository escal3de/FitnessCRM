using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain.Enums.Users;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Phone)
            .Must(x => x!.StartsWith('+')).WithMessage("Номер должен начинаться с '+'")
            .Matches(@"^\+\d{10,15}$").WithMessage("Некорректный формат номера")
            .When(x => x.Phone is not null);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Некорректный формат email")
            .When(x => x.Email is not null);

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов")
            .When(x => x.Name is not null);

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов")
            .When(x => x.LastName is not null);

        RuleFor(x => x.Patronymic)
            .MaximumLength(100).WithMessage("Отчество не должно превышать 100 символов")
            .When(x => x.Patronymic is not null);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2048).WithMessage("URL аватара не должен превышать 2048 символов")
            .When(x => x.AvatarUrl is not null);

        RuleFor(x => x.Birthday)
            .LessThan(DateTime.UtcNow).WithMessage("Дата рождения не может быть в будущем")
            .When(x => x.Birthday is not null);

        RuleFor(x => x.Profession)
            .MaximumLength(200).WithMessage("Профессия не должна превышать 200 символов")
            .When(x => x.Profession is not null);

        RuleFor(x => x.Gender)
            .Must(x => Enum.TryParse<UserGender>(x, true, out _))
            .WithMessage("Допустимые значения: Male, Female")
            .When(x => x.Gender is not null);
    }
}