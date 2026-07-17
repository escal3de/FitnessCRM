using FitnessCRM.Application.Contracts.Users;
using FluentValidation;

namespace FitnessCRM.Application.Validators.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        
    }
}