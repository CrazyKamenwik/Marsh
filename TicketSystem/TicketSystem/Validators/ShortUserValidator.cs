using FluentValidation;
using TicketSystem.API.ViewModels.Users;

namespace TicketSystem.API.Validators;

public class ShortUserValidator : AbstractValidator<ShortUserViewModel>
{
    public ShortUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("User name can't be empty")
            .Length(2, 50)
            .WithMessage("Name length can't be less than 2 or more than 50 symbols");

        RuleFor(x => x.UserRole)
            .NotEmpty()
            .WithMessage("User role can't be empty");
    }
}