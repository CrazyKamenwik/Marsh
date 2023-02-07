using TicketSystem.Data.Models.Enums;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public int TicketCreatorId { get; set; }
        public User TicketCreator { get; set; } = null!;

        public int? OperatorId { get; set; }
        public User? Operator { get; set; }

        public ICollection<Message> Messages { get; set; } = null!;
    }
}