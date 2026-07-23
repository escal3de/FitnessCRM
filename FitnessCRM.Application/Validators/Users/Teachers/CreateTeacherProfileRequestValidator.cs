using FitnessCRM.Application.Contracts.Users.Teachers;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users.Teachers;

public class CreateTeacherProfileRequestValidator : AbstractValidator<CreateTeacherProfileRequest>
{
    public CreateTeacherProfileRequestValidator()
    {
        RuleFor(x => x.PositionStatus)
            .NotEmpty().WithMessage("Статус должности обязателен")
            .Must(x => Enum.TryParse<Domain.Enums.Teacher.PositionStatus>(x, true, out _))
            .WithMessage("Некорректный статус должности");

        RuleFor(x => x.DirectionIds)
            .NotEmpty().WithMessage("Необходимо указать хотя бы одно направление");

        RuleFor(x => x.Achievements)
            .MaximumLength(5000).WithMessage("Достижения не должны превышать 5000 символов")
            .When(x => x.Achievements is not null);
    }
}
