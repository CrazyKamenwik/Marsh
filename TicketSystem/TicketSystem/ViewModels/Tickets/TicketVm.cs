using TicketSystem.ViewModels.Enums;
using TicketSystem.ViewModels.Messages;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.ViewModels.Tickets
{
    public class TicketVm
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatusEnumVm TicketStatus { get; set; }

        public UserVm TicketCreator { get; set; } = null!;
        public UserVm? Operator { get; set; }
        public ICollection<MessageVm> Messages { get; set; } = null!;
    }
}