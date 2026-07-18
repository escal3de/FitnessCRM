using FitnessCRM.Application.Contracts.Schools;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Schools;

public class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
{
    public CreateSchoolRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название школы обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Некорректный формат email");

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2048).WithMessage("URL аватара не должен превышать 2048 символов")
            .When(x => x.AvatarUrl is not null);
    }
}