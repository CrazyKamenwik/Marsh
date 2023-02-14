using System.Data;
using FluentValidation;
using TicketSystem.ViewModels.Messages;

namespace TicketSystem.Validators
{
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
}
