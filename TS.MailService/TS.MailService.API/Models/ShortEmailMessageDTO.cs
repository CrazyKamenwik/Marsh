using TS.MailService.Application.Enums;

namespace TS.MailService.Application.Models
{
    public class ShortEmailMessageDto
    {
        public string Sender { get; set; } = null!;
        public IEnumerable<string> Recipients { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Body { get; set; }
        public IEnumerable<object>? Attachments { get; set; }
    }
}