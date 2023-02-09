namespace TicketSystem.BLL.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public UserRoleModel UserRole { get; set; } = null!;
    public ICollection<TicketModel>? Tickets { get; set; }
}