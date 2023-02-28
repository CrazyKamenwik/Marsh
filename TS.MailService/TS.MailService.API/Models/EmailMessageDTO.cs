using TS.MailService.Application.Enums;

namespace TS.MailService.Application.Models;

public class EmailMessageDto
{
    public int Id { get; set; }
    public string Sender { get; set; } = null!;
    public IEnumerable<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Body { get; set; }
    public IEnumerable<object>? Attachments { get; set; }
    public EmailStatusDto EmailStatusEnum { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }

    public string EmailStatus => EmailStatusEnum.ToString();
}