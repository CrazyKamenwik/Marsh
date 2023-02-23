using TicketSystem.API.Enums;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.API.ViewModels.Users;

namespace TicketSystem.API.ViewModels.Tickets;

public class TicketViewModel
{
    public int Id { get; set; }
    public TicketStatusEnumViewModel TicketStatus { get; set; }

    public UserViewModel TicketCreator { get; set; } = null!;
    public UserViewModel? Operator { get; set; }
    public ICollection<MessageViewModel> Messages { get; set; } = null!;
}