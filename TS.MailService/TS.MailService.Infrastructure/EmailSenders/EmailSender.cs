using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TS.MailService.Infrastructure.Abstraction.EmailSenders;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Infrastructure.EmailSenders;

internal class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(IOptions<SmtpEntity> smtp)
    {
        var smtpEntity = smtp.Value;
        _smtpClient = new SmtpClient(smtpEntity.Host, smtpEntity.Port);
        _smtpClient.EnableSsl = true;
        _smtpClient.Credentials = new NetworkCredential(smtpEntity.Username, smtpEntity.Password);
    }

    public async Task SendEmail(MailMessage message, CancellationToken cancellationToken)
    {
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }
}