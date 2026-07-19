using FitnessCRM.Application.Contracts.Users.Clients;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users.Clients;

public class CreateClientProfileRequestValidator : AbstractValidator<CreateClientProfileRequest>
{
    public CreateClientProfileRequestValidator()
    {
        RuleFor(x => x.Height)
            .InclusiveBetween(50, 300).WithMessage("Рост должен быть от 50 до 300 см")
            .When(x => x.Height is not null);

        RuleFor(x => x.Weight)
            .InclusiveBetween(20, 500).WithMessage("Вес должен быть от 20 до 500 кг")
            .When(x => x.Weight is not null);
    }
}
