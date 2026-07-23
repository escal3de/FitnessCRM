using FitnessCRM.Application.Contracts.ContactData;
using FluentValidation;

namespace FitnessCRM.Application.Validators.ContactData;

public class CreateContactDataRequestValidator : AbstractValidator<CreateContactDataRequest>
{
    public CreateContactDataRequestValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Номер телефона обязателен")
            .Must(x => x.StartsWith('+')).WithMessage("Номер должен начинаться с '+'")
            .Matches(@"^\+\d{10,15}$").WithMessage("Некорректный формат номера");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна")
            .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов");

        RuleFor(x => x.Patronymic)
            .NotEmpty().WithMessage("Отчество обязательно")
            .MaximumLength(100).WithMessage("Отчество не должно превышать 100 символов");

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2048).WithMessage("URL аватара не должен превышать 2048 символов")
            .When(x => x.AvatarUrl is not null);

        RuleFor(x => x.Birthday)
            .LessThan(DateTime.UtcNow).WithMessage("Дата рождения не может быть в будущем")
            .When(x => x.Birthday is not null);

        RuleFor(x => x.Profession)
            .MaximumLength(200).WithMessage("Профессия не должна превышать 200 символов")
            .When(x => x.Profession is not null);
    }
}
