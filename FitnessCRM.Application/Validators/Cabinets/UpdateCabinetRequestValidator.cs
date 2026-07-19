using FitnessCRM.Application.Contracts.Cabinets;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Cabinets;

public class UpdateCabinetRequestValidator : AbstractValidator<UpdateCabinetRequest>
{
    public UpdateCabinetRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название кабинета не может быть пустым")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не должно превышать 2000 символов")
            .When(x => x.Description is not null);

        RuleForEach(x => x.MediaUrls)
            .MaximumLength(2048).WithMessage("URL медиа не должен превышать 2048 символов")
            .When(x => x.MediaUrls is not null);
    }
}
