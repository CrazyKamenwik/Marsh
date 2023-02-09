using TicketSystem.ViewModels.Enums;
using TicketSystem.ViewModels.Messages;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.ViewModels.Tickets;

public class TicketViewModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public TicketStatusEnumVm TicketStatus { get; set; }

    public UserViewModel TicketCreator { get; set; } = null!;
    public UserViewModel? Operator { get; set; }
    public ICollection<MessageViewModel> Messages { get; set; } = null!;
}