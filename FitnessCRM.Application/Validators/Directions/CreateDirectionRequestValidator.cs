using FitnessCRM.Application.Contracts.Directions;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Directions;

public class CreateDirectionRequestValidator : AbstractValidator<CreateDirectionRequest>
{
    public CreateDirectionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название направления обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MaximumLength(2000).WithMessage("Описание не должно превышать 2000 символов");
    }
}
