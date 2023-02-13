using TicketSystem.API.ViewModels.Users;

namespace TicketSystem.API.ViewModels.Messages;

public class MessageViewModel
{
    public int Id { get; set; }
    public string? Text { get; set; }

    public UserViewModel User { get; set; } = null!;
}