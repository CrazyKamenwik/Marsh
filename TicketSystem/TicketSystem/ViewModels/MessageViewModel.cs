namespace TicketSystem.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserViewModel User { get; set; } = null!;
    }
}