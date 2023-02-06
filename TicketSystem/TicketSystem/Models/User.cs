namespace TicketSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public UserRole Role { get; set; } = null!;
        public ICollection<Ticket>? Tickets { get; set; }
    }
}