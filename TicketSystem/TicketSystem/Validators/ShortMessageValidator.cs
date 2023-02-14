using FluentValidation;
using TicketSystem.API.ViewModels.Messages;

namespace TicketSystem.API.Validators;

public class ShortMessageValidator : AbstractValidator<ShortMessageViewModel>
{
    public ShortMessageValidator()
    {
        RuleFor(x => x.Text)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message text can't be null or empty");
    }
}