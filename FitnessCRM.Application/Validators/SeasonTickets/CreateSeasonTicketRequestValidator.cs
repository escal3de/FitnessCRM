using FitnessCRM.Application.Contracts.SeasonTickets;
using FluentValidation;

namespace FitnessCRM.Application.Validators.SeasonTickets;

public class CreateSeasonTicketRequestValidator : AbstractValidator<CreateSeasonTicketRequest>
{
    public CreateSeasonTicketRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Идентификатор клиента обязателен");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.ActiveAt)
            .NotEmpty().WithMessage("Дата активации обязательна");

        RuleFor(x => x.ExpiresAt)
            .NotEmpty().WithMessage("Дата окончания обязательна")
            .GreaterThan(x => x.ActiveAt).WithMessage("Дата окончания должна быть позже даты активации");

        RuleFor(x => x.RemainingWorkouts)
            .GreaterThanOrEqualTo(0).WithMessage("Количество тренировок не может быть отрицательным");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Цена не может быть отрицательной");

        RuleFor(x => x.SchoolName)
            .NotEmpty().WithMessage("Название школы обязательно")
            .MaximumLength(200).WithMessage("Название школы не должно превышать 200 символов");
    }
}