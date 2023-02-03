namespace TicketSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; } = null!;
        public Ticket Ticket { get; set; } = null!;
    }
}