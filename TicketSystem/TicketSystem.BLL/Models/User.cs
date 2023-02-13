namespace TicketSystem.BLL.Models;

public class User : BaseModel
{
    public string Name { get; set; } = null!;

    public UserRole UserRole { get; set; } = null!;
    public ICollection<Ticket>? Tickets { get; set; }
}