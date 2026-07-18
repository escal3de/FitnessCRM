using FitnessCRM.Application.Contracts.Schools;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Schools;

public class UpdateSchoolRequestValidator : AbstractValidator<UpdateSchoolRequest>
{
    public UpdateSchoolRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название школы не может быть пустым")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .When(x => x.Name is not null);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Некорректный формат email")
            .When(x => x.Email is not null);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2048).WithMessage("URL аватара не должен превышать 2048 символов")
            .When(x => x.AvatarUrl is not null);
    }
}