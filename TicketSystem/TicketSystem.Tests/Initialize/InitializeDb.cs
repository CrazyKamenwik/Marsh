using TicketSystem.BLL.Constants;
using TicketSystem.DAL;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.Tests.Initialize;

public static class InitializeDb
{
    public static void Initialize(ApplicationContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(InitializeData.GetAllUsersEntities());
        context.Tickets.AddRange(InitializeData.GetAllTicketsEntities());

        context.SaveChangesAsync();
    }
}