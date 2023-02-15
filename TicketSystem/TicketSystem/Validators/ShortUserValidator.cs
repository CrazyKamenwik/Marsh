using FluentValidation;
using TicketSystem.API.ViewModels.Users;

namespace TicketSystem.API.Validators;

public class ShortUserValidator : AbstractValidator<ShortUserViewModel>
{
    public ShortUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.UserRole)
            .NotEmpty();
    }
}