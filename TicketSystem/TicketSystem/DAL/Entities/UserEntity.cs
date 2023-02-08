namespace TicketSystem.DAL.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public UserRoleEntity UserRole { get; set; } = null!;

        public ICollection<TicketEntity>? Tickets { get; set; }
    }
}