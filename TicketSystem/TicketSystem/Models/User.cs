namespace TicketSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public UserRole Role { get; set; }
    }
}