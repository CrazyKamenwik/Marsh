using AutoMapper;
using Moq;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.MessagesStrategy;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.MessageStrategyTests;

public class UserMessageStrategyTest
{
    private readonly Mock<IGenericRepository<MessageEntity>> _messageRepositoryMock;
    private readonly IMessageStrategy _messageStrategy;

    public UserMessageStrategyTest()
    {
        var ticketServiceMock = new Mock<ITicketService>();
        var mapperMock = MapperMock.GetMapperMock();

        _messageRepositoryMock = GenericRepositoryMock<MessageEntity>.GetMock(new List<MessageEntity>
            { InitializeData.GetMessageEntityFromUser() });
        _messageStrategy = new UserMessageStrategy(ticketServiceMock.Object,
            _messageRepositoryMock.Object, mapperMock.Object);
    }

    [Theory]
    [InlineData(RolesConstants.Operator, false)]
    [InlineData(RolesConstants.User, true)]
    public void IsApplicable_StringIsOperator_ReturnTrue(string userRole, bool expected)
    {
        // Act
        var result = _messageStrategy.IsApplicable(userRole);

        // Assert
        Assert.Equal(result, expected);
    }

    [Fact]
    public async Task AddMessageAsync_ShouldAddMessageToOpenTicketAndReturnMessage_WhenUserHasOpenTicket()
    {
        // Arrange
        var user = InitializeData.GetUserModelUserWithOpenTicket();
        var message = InitializeData.GetMessageModelFromUser();

        // Act
        var result = await _messageStrategy.AddMessage(message, user, CancellationToken.None);

        // Assert
        Assert.IsType<Message>(result);
        _messageRepositoryMock.Verify(x =>
            x.Create(It.IsAny<MessageEntity>(), CancellationToken.None), Times.Once);
    }
}