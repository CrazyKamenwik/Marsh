using TicketSystem.BLL.Constants;
using TicketSystem.DAL;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.Tests.IntergationTests.InitializeModels;

public static class InitializeDb
{
    public static void Initialize(ApplicationContext context)
    {
        context.Users.AddRange(GetUsers());
        context.Tickets.AddRange(GetTickets());
        context.Messages.AddRange(GetMessages());
    }

    private static IEnumerable<UserEntity> GetUsers()
    {
        return new List<UserEntity>()
        {
            new()
            {
                Id = 1,
                Name = "Vlad",
                UserRole = new UserRoleEntity()
                {
                    Name = RolesConstants.User
                }
            },
            new()
            {
                Id = 2,
                Name = "Tanya",
                UserRole = new UserRoleEntity()
                {
                    Name = RolesConstants.Operator
                }
            },
        };
    }

    private static IEnumerable<TicketEntity> GetTickets()
    {
        var openTicketEntity = new TicketEntity()
        {
            Id = 1,
            CreatedAt = DateTime.Now - TimeSpan.FromHours(1),
            Messages = new List<MessageEntity>(),
            OperatorId = 2,
            TicketCreatorId = 1,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        var closeTicketEntity = new TicketEntity()
        {
            Id = 1,
            CreatedAt = DateTime.Now - TimeSpan.FromHours(2),
            Messages = new List<MessageEntity>(),
            OperatorId = 2,
            TicketCreatorId = 1,
            TicketStatus = TicketStatusEnumEntity.Closed
        };

        return new List<TicketEntity>() { openTicketEntity, closeTicketEntity };
    }

    private static IEnumerable<MessageEntity> GetMessages()
    {
        return new List<MessageEntity>()
        {
            new()
            {
                CreatedAt = DateTime.Now - TimeSpan.FromMinutes(50),
                TicketId = 1,
                UserId = 1,
                Text = "Hello"
            },
            new()
            {
                CreatedAt = DateTime.Now - TimeSpan.FromMinutes(30),
                TicketId = 1,
                UserId = 2,
                Text = "Hello man!"
            }
        };
    }
}