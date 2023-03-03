namespace TS.MailService.Infrastructure.Entities;

public class EmailMessageEntity : BaseEntity
{
    public string Sender { get; set; } = null!;
    public IEnumerable<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
}