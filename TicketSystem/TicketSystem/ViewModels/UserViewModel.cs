namespace TicketSystem.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public UserRoleViewModel UserRole { get; set; } = null!;
    }
}