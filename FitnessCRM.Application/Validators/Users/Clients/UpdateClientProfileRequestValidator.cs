using FitnessCRM.Application.Contracts.Users.Clients;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users.Clients;

public class UpdateClientProfileRequestValidator : AbstractValidator<UpdateClientProfileRequest>
{
    public UpdateClientProfileRequestValidator()
    {
        RuleFor(x => x.Height)
            .InclusiveBetween(50, 300).WithMessage("Рост должен быть от 50 до 300 см")
            .When(x => x.Height is not null);

        RuleFor(x => x.Weight)
            .InclusiveBetween(20, 500).WithMessage("Вес должен быть от 20 до 500 кг")
            .When(x => x.Weight is not null);

        RuleFor(x => x.MedicalContraindications)
            .MaximumLength(2000).WithMessage("Медицинские противопоказания не должны превышать 2000 символов")
            .When(x => x.MedicalContraindications is not null);

        RuleFor(x => x.ParentsCommunication)
            .MaximumLength(500).WithMessage("Контакт родителя не должен превышать 500 символов")
            .When(x => x.ParentsCommunication is not null);
    }
}
