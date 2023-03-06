using FastEndpoints;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Models;

namespace TS.MailService.Application.Mappers.EmailMappers;

public class EmailWithRequestMapper : Mapper<ShortEmailMessageDto, EmailMessageDto, EmailMessage>
{
    public override EmailMessage ToEntity(ShortEmailMessageDto shortEmail) => new()
    {
        Sender = shortEmail.Sender,
        Subject = shortEmail.Subject,
        Body = shortEmail.Body,
        Recipients = shortEmail.Recipients,
    };

    public override EmailMessageDto FromEntity(EmailMessage emailModel) => new()
    {
        Id = emailModel.Id,
        Body = emailModel.Body,
        CreatedAt = emailModel.CreatedAt,
        Recipients = emailModel.Recipients,
        Sender = emailModel.Sender,
        Subject = emailModel.Subject
    };
}