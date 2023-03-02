using FastEndpoints;
using TS.MailService.Application.Enums;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Models;

namespace TS.MailService.Application.Mappers.EmailMappers;

public class EmailWithoutRequestMapper : ResponseMapper<EmailMessageDto, EmailMessage>
{
    public override EmailMessageDto FromEntity(EmailMessage emailMessage) => new()
    {
        Id = emailMessage.Id,
        Body = emailMessage.Body,
        CreatedAt = emailMessage.CreatedAt,
        Recipients = emailMessage.Recipients,
        Sender = emailMessage.Sender,
        Subject = emailMessage.Subject
    };
}