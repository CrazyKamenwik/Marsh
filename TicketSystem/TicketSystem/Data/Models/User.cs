namespace TicketSystem.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public UserRole UserRole { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}