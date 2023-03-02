using TS.MailService.Domain.Models;

namespace TS.MailService.Domain.Abstraction.Services;

public interface IEmailService
{
    Task<EmailMessage> SendEmail(EmailMessage message, CancellationToken cancellationToken);
    Task<EmailMessage> GetEmail(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<EmailMessage>> GetEmails(CancellationToken cancellationToken);
}