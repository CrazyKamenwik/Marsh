using FluentValidation;
using TicketSystem.API.ViewModels.Messages;

namespace TicketSystem.API.Validators;

public class ShortMessageValidator : AbstractValidator<ShortMessageViewModel>
{
    public ShortMessageValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}