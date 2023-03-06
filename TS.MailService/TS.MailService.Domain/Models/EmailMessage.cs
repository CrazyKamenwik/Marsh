using TS.MailService.Domain.Enums;

namespace TS.MailService.Domain.Models;

public class EmailMessage : BaseModel
{
    public string Sender { get; set; } = null!;
    public IEnumerable<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Body { get; set; }
    public IEnumerable<object>? Attachments { get; set; }
    public EmailStatus EmailStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}