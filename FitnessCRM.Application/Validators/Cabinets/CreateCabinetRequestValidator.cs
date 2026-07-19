using FitnessCRM.Application.Contracts.Cabinets;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Cabinets;

public class CreateCabinetRequestValidator : AbstractValidator<CreateCabinetRequest>
{
    public CreateCabinetRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название кабинета обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не должно превышать 2000 символов")
            .When(x => x.Description is not null);

        RuleFor(x => x.MediaUrls)
            .NotNull().WithMessage("Список медиа не может быть null");

        RuleForEach(x => x.MediaUrls)
            .MaximumLength(2048).WithMessage("URL медиа не должен превышать 2048 символов");
    }
}
