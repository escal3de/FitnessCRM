using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Domain.Enums.SeasonTicket;
using FluentValidation;

namespace FitnessCRM.Application.Validators.SeasonTickets;

public class CreateSeasonTicketTemplateRequestValidator : AbstractValidator<CreateSeasonTicketTemplateRequest>
{
    public CreateSeasonTicketTemplateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Тип обязателен")
            .Must(x => Enum.TryParse<TicketType>(x, true, out _))
            .WithMessage("Допустимые значения: OneTime, Limited, Unlimited");

        RuleFor(x => x.ValidityPeriod)
            .GreaterThan(0).WithMessage("Период действия должен быть больше 0");

        RuleFor(x => x.WorkoutAmount)
            .GreaterThan(0).WithMessage("Количество тренировок должно быть больше 0");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate ?? DateTime.MinValue)
            .WithMessage("Дата окончания должна быть позже даты начала");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Дата начала не может быть в прошлом")
            .When(x => x.StartDate is not null);
    }
}