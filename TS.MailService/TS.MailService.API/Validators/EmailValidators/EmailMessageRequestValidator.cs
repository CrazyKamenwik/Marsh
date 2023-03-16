using FastEndpoints;
using FluentValidation;
using TS.MailService.Application.Models;

namespace TS.MailService.Application.Validators.EmailValidators;

public class EmailMessageRequestValidator : Validator<ShortEmailMessageDto>
{
    public EmailMessageRequestValidator()
    {
        RuleFor(e => e.Sender)
            .NotEmpty()
            .EmailAddress();

        RuleForEach(e => e.Recipients)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Subject)
            .NotEmpty();
    }
}