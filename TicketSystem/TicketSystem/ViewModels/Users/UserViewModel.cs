namespace TicketSystem.API.ViewModels.Users;

public class UserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string UserRole { get; set; } = null!;
}