using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.MailService.Infrastructure.Entities
{
    internal class EmailMessageEntity
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
}
