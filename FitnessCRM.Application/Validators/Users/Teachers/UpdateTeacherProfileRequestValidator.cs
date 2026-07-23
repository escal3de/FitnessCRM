using FitnessCRM.Application.Contracts.Users.Teachers;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users.Teachers;

public class UpdateTeacherProfileRequestValidator : AbstractValidator<UpdateTeacherProfileRequest>
{
    public UpdateTeacherProfileRequestValidator()
    {
        RuleFor(x => x.PositionStatus)
            .Must(x => Enum.TryParse<Domain.Enums.Teacher.PositionStatus>(x, true, out _))
            .WithMessage("Некорректный статус должности")
            .When(x => x.PositionStatus is not null);

        RuleFor(x => x.Achievements)
            .MaximumLength(5000).WithMessage("Достижения не должны превышать 5000 символов")
            .When(x => x.Achievements is not null);
    }
}
