using TS.MailService.Domain.Enums;

namespace TS.MailService.Domain.Models;

public class EmailMessage : BaseModel
{
    public string Sender { get; set; } = null!;
    public IEnumerable<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}