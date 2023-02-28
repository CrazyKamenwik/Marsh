using TS.MailService.Infrastructure.Enums;

namespace TS.MailService.Infrastructure.Entities;

internal class EmailMessageEntity : BaseEntity
{
    public string Sender { get; set; } = null!;
    public IEnumerable<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Body { get; set; }
    public IEnumerable<object>? Attachments { get; set; }
    public EmailStatusEntity EmailStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}