using AutoMapper;
using Moq;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;

namespace TicketSystem.Tests.UnitTests.ServicesTests;

public class MessageServiceTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMessageService _messageService;
    private readonly Mock<IEnumerable<IMessageStrategy>> _messageStrategiesMock;

    private readonly Mock<IMessageStrategy> _userMessageStrategyMock;
    private readonly Mock<IUserService> _userServiceMock;

    public MessageServiceTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _mapperMock = new Mock<IMapper>();
        _messageStrategiesMock = new Mock<IEnumerable<IMessageStrategy>>();
        _userMessageStrategyMock = new Mock<IMessageStrategy>();

        _messageService =
            new MessageService(_userServiceMock.Object, _mapperMock.Object, _messageStrategiesMock.Object);
    }

    [Fact]
    public async Task Add_UserRoleIsUser_ReturnMessageAndCallCorrectMessageStrategy()
    {
        // Arrange
        var message = new Message { Text = "Yup" };

        _mapperMock.Setup(x => x.Map<Message>(It.IsAny<Message>()))
            .Returns(new Message());
        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new User
                { Id = 1, Name = "Vlad", UserRole = new UserRole { Id = 1, Name = RolesConstants.User } });
        _userMessageStrategyMock.Setup(x => x.IsApplicable(It.IsAny<string>())).Returns(true);
        _messageStrategiesMock.Setup(x => x.GetEnumerator())
            .Returns(new List<IMessageStrategy>
            {
                _userMessageStrategyMock.Object
            }.GetEnumerator());

        // Act
        var result = await _messageService.AddMessage(message, CancellationToken.None);

        // Assert
        Assert.IsType<Message>(result);
        _userServiceMock.Verify(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None));
        _messageStrategiesMock.Verify(x => x.GetEnumerator(), Times.Once);
        _userMessageStrategyMock.Verify(x => x.IsApplicable(It.IsAny<string>()), Times.Once);
        _userMessageStrategyMock.Verify(
            x => x.AddMessage(It.IsAny<Message>(), It.IsAny<User>(), CancellationToken.None));
    }

    [Fact]
    public async Task Add_UserRoleIsOperatorAndMessageHaveNoTicketId_ReturnArgumentExceptionWithMessage()
    {
        // Arrange
        var message = new Message { Text = "Yup" };

        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new User
                { Id = 1, Name = "Vlad", UserRole = new UserRole { Id = 1, Name = RolesConstants.Operator } });

        // Act
        var result =
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _messageService.AddMessage(message, CancellationToken.None));

        // Assert
        Assert.IsType<ArgumentException>(result);
        Assert.NotEmpty(result.Message);
        _userServiceMock.Verify(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None));
    }
}