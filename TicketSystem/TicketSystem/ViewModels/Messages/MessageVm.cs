using TicketSystem.ViewModels.Users;

namespace TicketSystem.ViewModels.Messages
{
    public class MessageVm
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserVm User { get; set; } = null!;
    }
}