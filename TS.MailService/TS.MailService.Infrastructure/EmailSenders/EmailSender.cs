using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TS.MailService.Infrastructure.Abstraction.EmailSenders;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Infrastructure.EmailSenders;

internal class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(IConfiguration configuration)
    {
        var emailSender = configuration["Smtp:Username"];
        var senderPassword = configuration["Smtp:Password"];
        var smtpHost = configuration["Smtp:Host"];
        var smtpPort = int.Parse(configuration["Smtp:Port"] ?? throw new ArgumentException());

        _smtpClient = new SmtpClient(smtpHost, smtpPort);
        _smtpClient.EnableSsl = true;
        _smtpClient.Credentials = new NetworkCredential(emailSender, senderPassword);
    }

    public async Task SendEmail(MailMessage message)
    {
        await _smtpClient.SendMailAsync(message);
    }
}