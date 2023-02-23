namespace TicketSystem.DAL.Entities;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = null!;

    public int UserRoleId { get; set; }
    public UserRoleEntity UserRole { get; set; } = null!;
    public ICollection<TicketEntity>? Tickets { get; set; }
}