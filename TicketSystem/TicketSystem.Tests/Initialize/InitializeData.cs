using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.API.Enums;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.API.ViewModels.Tickets;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.Tests.Initialize;
public static class InitializeData
{
    private const string TextMessage = "Hello";

    private const int UserId = 1;
    private const string UserName = "Vlad";

    private const int OperatorId = 2;
    private const string OperatorName = "Tanya";

    private const int UserRoleId = 1;
    private const int OperatorRoleId = 2;

    private const int MessageIdFromUser = 1;
    private const int MessageIdFromOperator = 2;

    private const int TicketId = 1;

    #region ShortModels
    public static ShortMessageViewModel GetShortMessageViewModelFromUser() => new()
    {
        Text = TextMessage,
        UserId = UserId
    };

    public static ShortMessageViewModel GetShortMessageViewModelFromOperator() => new()
    {
        Text = TextMessage,
        UserId = OperatorId,
        TicketId = TicketId
    };

    public static ShortUserViewModel GetShortUserViewModelUser() => new()
    {
        Name = UserName,
        UserRole = RolesConstants.User
    };

    public static ShortUserViewModel GetShortUserViewModelOperator() => new()
    {
        Name = OperatorName,
        UserRole = RolesConstants.Operator
    };
    #endregion

    #region ViewModels

    public static MessageViewModel GetMessageViewModelFromUser() => new()
    {
        Id = MessageIdFromUser,
        Text = TextMessage,
        User = GetUserViewModelUser()
    };

    public static MessageViewModel GetMessageViewModelFromOperator() => new()
    {
        Id = MessageIdFromOperator,
        Text = TextMessage,
        User = GetUserViewModelUser()
    };

    public static UserViewModel GetUserViewModelUser() => new()
    {
        Id = UserId,
        Name = UserName,
        UserRole = RolesConstants.User
    };

    public static UserViewModel GetUserViewModelOperator() => new()
    {
        Id = OperatorId,
        Name = OperatorName,
        UserRole = RolesConstants.Operator
    };

    public static IEnumerable<UserViewModel> GetAllUsersViewModel() => new List<UserViewModel>()
    {
        GetUserViewModelUser(),
        GetUserViewModelOperator()
    };

    public static TicketViewModel GetTicketViewModel() => new()
    {
        Id = TicketId,
        Messages = new List<MessageViewModel>()
        {
            GetMessageViewModelFromUser(),
            GetMessageViewModelFromOperator()
        },
        Operator = GetUserViewModelOperator(),
        TicketCreator = GetUserViewModelUser(),
        TicketStatus = TicketStatusEnumViewModel.Open
    };

    public static IEnumerable<TicketViewModel> GetAllTicketsViewModel() => new List<TicketViewModel>()
    {
        GetTicketViewModel()
    };

    #endregion

    #region Models
    public static Message GetMessageModelFromUser() => new()
    {
        Id = MessageIdFromUser,
        CreatedAt = DateTime.Now,
        Text = TextMessage,
        TicketId = TicketId,
        UserId = UserId
    };

    public static Message GetMessageModelFromOperator() => new()
    {
        Id = MessageIdFromOperator,
        CreatedAt = DateTime.Now,
        Text = TextMessage,
        TicketId = TicketId,
        UserId = OperatorId
    };

    public static Message GetMessageModelFromOperatorWithoutTicketId() => new()
    {
        Id = MessageIdFromOperator,
        CreatedAt = DateTime.Now,
        Text = TextMessage,
        UserId = OperatorId
    };

    public static User GetUserModelUser() => new()
    {
        Id = UserId,
        Name = UserName,
        UserRole = new UserRole
        {
            Id = UserRoleId,
            Name = RolesConstants.User
        }
    };

    public static User GetUserModelUserWithOpenTicket() => new()
    {
        Id = UserId,
        Name = UserName,
        Tickets = new List<Ticket>()
        {
            GetTicketModel()
        },
        UserRole = new UserRole
        {
            Id = UserRoleId,
            Name = RolesConstants.User
        }
    };

    public static User GetUserModelOperator() => new()
    {
        Id = OperatorId,
        Name = OperatorName,
        UserRole = new UserRole
        {
            Id = OperatorRoleId,
            Name = RolesConstants.Operator
        }
    };

    public static IEnumerable<User> GetAllUsersModels() => new List<User>()
    {
        GetUserModelUser(),
        GetUserModelOperator()
    };

    public static Ticket GetTicketModel() => new(UserId)
    {
        Id = TicketId,
        CreatedAt = DateTime.Now,
        Messages = new List<Message>()
        {
            GetMessageModelFromUser(),
            GetMessageModelFromOperator()
        },
        OperatorId = OperatorId,
        TicketStatus = TicketStatusEnumModel.Open
    };

    public static IEnumerable<Ticket> GetAllTicketsModels() => new List<Ticket>()
    {
        GetTicketModel()
    };

    public static IEnumerable<TicketEntity> GetTicketsThatCanBeClosed()
    {
        var userEntityUser = GetUserEntityUser();
        var userEntityOperator = GetUserEntityOperator();

        var firstTicketEntity = new TicketEntity
        {
            Id = 1,
            CreatedAt = DateTime.Now - TimeSpan.FromMinutes(70),
            Messages = new List<MessageEntity> { new() { CreatedAt = DateTime.Now - TimeSpan.FromMinutes(61) } },
            TicketCreator = userEntityUser,
            OperatorId = OperatorId,
            Operator = userEntityOperator,
            TicketCreatorId = UserId,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        var secondTicketEntity = new TicketEntity
        {
            Id = 2,
            CreatedAt = DateTime.Now - TimeSpan.FromMinutes(120),
            Messages = new List<MessageEntity> { new() { CreatedAt = DateTime.Now - TimeSpan.FromMinutes(90) } },
            TicketCreator = userEntityUser,
            OperatorId = 1,
            Operator = userEntityOperator,
            TicketCreatorId = UserId,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        return new List<TicketEntity> { firstTicketEntity, secondTicketEntity };
    }
    #endregion

    #region Entities
    public static MessageEntity GetMessageEntityFromUser() => new()
    {
        Id = MessageIdFromUser,
        CreatedAt = DateTime.Now,
        Text = TextMessage,
        TicketId = TicketId,
        UserId = UserId
    };

    public static MessageEntity GetMessageEntityFromOperator() => new()
    {
        Id = MessageIdFromOperator,
        CreatedAt = DateTime.Now,
        Text = TextMessage,
        TicketId = TicketId,
        UserId = OperatorId
    };

    public static UserEntity GetUserEntityUser() => new()
    {
        Id = UserId,
        Name = UserName,
        UserRole = new UserRoleEntity
        {
            Id = UserRoleId,
            Name = RolesConstants.User
        }
    };

    public static UserEntity GetUserEntityOperator() => new()
    {
        Id = OperatorId,
        Name = OperatorName,
        UserRole = new UserRoleEntity
        {
            Id = OperatorRoleId,
            Name = RolesConstants.Operator
        }
    };

    public static IEnumerable<UserEntity> GetAllUsersEntities() => new List<UserEntity>()
        {
            GetUserEntityUser(),
            GetUserEntityOperator()
        };

    public static TicketEntity GetTicketEntity() => new()
    {
        Id = TicketId,
        CreatedAt = DateTime.Now,
        Messages = new List<MessageEntity>()
            {
                GetMessageEntityFromUser(),
                GetMessageEntityFromOperator()
            },
        OperatorId = OperatorId,
        TicketCreatorId = UserId,
        TicketStatus = TicketStatusEnumEntity.Open
    };

    public static IEnumerable<TicketEntity> GetAllTicketsEntities() => new List<TicketEntity>()
    {
        GetTicketEntity()
    };

    public static IEnumerable<MessageEntity> GetAllMessagesEntities() => new List<MessageEntity>()
    {
        GetMessageEntityFromUser(),
        GetMessageEntityFromOperator()
    };
    #endregion
}