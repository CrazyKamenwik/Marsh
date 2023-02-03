using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace TicketSystem.Models
{
    public class ApplicationContext : DbContext
    {
        public  DbSet<User> Users { get; set; }
        public  DbSet<Ticket> Tickets { get; set; }
        public  DbSet<Message> Messages { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=warehouse;Trusted_Connection=True;");
        }
    }
}
