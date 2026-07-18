using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Domain.Enums.SeasonTicket;
using FluentValidation;

namespace FitnessCRM.Application.Validators.SeasonTickets;

public class UpdateSeasonTicketTemplateRequestValidator : AbstractValidator<UpdateSeasonTicketTemplateRequest>
{
    public UpdateSeasonTicketTemplateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .When(x => x.Name is not null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0")
            .When(x => x.Price is not null);

        RuleFor(x => x.Type)
            .Must(x => Enum.TryParse<TicketType>(x, true, out _))
            .WithMessage("Допустимые значения: OneTime, Limited, Unlimited")
            .When(x => x.Type is not null);

        RuleFor(x => x.ValidityPeriod)
            .GreaterThan(0).WithMessage("Период действия должен быть больше 0")
            .When(x => x.ValidityPeriod is not null);

        RuleFor(x => x.WorkoutAmount)
            .GreaterThan(0).WithMessage("Количество тренировок должно быть больше 0")
            .When(x => x.WorkoutAmount is not null);
    }
}