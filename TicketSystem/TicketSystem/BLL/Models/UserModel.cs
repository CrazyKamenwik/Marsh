namespace TicketSystem.BLL.Models;

public class UserModel : BaseModel
{
    public string Name { get; set; } = null!;

    public UserRoleModel UserRole { get; set; } = null!;
    public ICollection<TicketModel>? Tickets { get; set; }
}