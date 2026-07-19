using FitnessCRM.Application.Contracts.Directions;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Directions;

public class UpdateDirectionRequestValidator : AbstractValidator<UpdateDirectionRequest>
{
    public UpdateDirectionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название направления не может быть пустым")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание не может быть пустым")
            .MaximumLength(2000).WithMessage("Описание не должно превышать 2000 символов")
            .When(x => x.Description is not null);
    }
}
