using TicketSystem.ViewModels.Enums;

namespace TicketSystem.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatusEnumViewModel TicketStatus { get; set; }

        public UserViewModel TicketCreator { get; set; } = null!;
        public UserViewModel? Operator { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; } = null!;
    }
}