namespace TicketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public TicketStatus TicketStatus { get; set; } = null!;

        public ICollection<User> Users { get; set; } = null!;
        public ICollection<Message>? Messages { get; set; }
    }
}