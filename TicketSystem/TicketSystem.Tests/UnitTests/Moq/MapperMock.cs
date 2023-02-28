using AutoMapper;
using Moq;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;

namespace TicketSystem.Tests.UnitTests.Moq;

internal class MapperMock
{
    public static Mock<IMapper> GetMapperMock()
    {
        var mapper = new Mock<IMapper>();

        // MessageEntity -> Message
        mapper.Setup(x => x.Map<Message>(It.IsAny<MessageEntity>()))
            .Returns(InitializeData.GetMessageModelFromUser());

        // Message -> MessageEntity
        mapper.Setup(x => x.Map<MessageEntity>(It.IsAny<Message>()))
            .Returns(InitializeData.GetMessageEntityFromUser);

        // UserEntity -> User
        mapper.Setup(x => x.Map<User>(It.IsAny<UserEntity>()))
            .Returns(InitializeData.GetUserModelUser());

        // User -> UserEntity
        mapper.Setup(x => x.Map<UserEntity>(It.IsAny<User>()))
            .Returns(InitializeData.GetUserEntityUser());

        // IEnumerable<UserEntity> -> IEnumerable<User>
        mapper.Setup(x => x.Map<IEnumerable<User>>(It.IsAny<IEnumerable<UserEntity>>()))
            .Returns(InitializeData.GetAllUsersModels());

        // IEnumerable<TicketEntity> -> IEnumerable<Ticket>
        mapper.Setup(x => x.Map<IEnumerable<Ticket>>(It.IsAny<IEnumerable<TicketEntity>>()))
            .Returns(InitializeData.GetAllTicketsModels());

        // Ticket -> TicketEntity
        mapper.Setup(x => x.Map<TicketEntity>(It.IsAny<Ticket>()))
            .Returns(InitializeData.GetTicketEntity());

        // TicketEntity -> Ticket
        mapper.Setup(x => x.Map<Ticket>(It.IsAny<TicketEntity>()))
            .Returns(InitializeData.GetTicketModel());

        return mapper;
    }
}