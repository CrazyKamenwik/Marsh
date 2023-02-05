namespace TicketSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
    }
}