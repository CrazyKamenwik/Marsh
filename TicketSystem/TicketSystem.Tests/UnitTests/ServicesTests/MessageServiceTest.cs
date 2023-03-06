using Moq;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.ServicesTests;

public class MessageServiceTest
{
    private readonly IMessageService _messageService;
    private readonly Mock<IEnumerable<IMessageStrategy>> _messageStrategiesMock;

    private readonly Mock<IMessageStrategy> _userMessageStrategyMock;
    private readonly Mock<IUserService> _userServiceMock;

    public MessageServiceTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _messageStrategiesMock = new Mock<IEnumerable<IMessageStrategy>>();
        _userMessageStrategyMock = new Mock<IMessageStrategy>();
        _messageService =
            new MessageService(_userServiceMock.Object, _messageStrategiesMock.Object);
    }

    [Fact]
    public async Task Add_UserRoleIsUser_ReturnMessageAndCallCorrectMessageStrategy()
    {
        // Arrange
        var message = InitializeData.GetMessageModelFromUser();
        var user = InitializeData.GetUserModelUser();

        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(user);

        _userMessageStrategyMock.Setup(x => x.AddMessage(message, user, CancellationToken.None))
            .ReturnsAsync(message);
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
        var messageWithoutTicketId = InitializeData.GetMessageModelFromOperatorWithoutTicketId();
        var userOperator = InitializeData.GetUserModelOperator();

        _userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(userOperator);


        // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.AddMessage(messageWithoutTicketId, CancellationToken.None));

        // Assert
        Assert.IsType<ArgumentException>(result);
        Assert.NotEmpty(result.Message);
        _userServiceMock.Verify(x => x.GetUserById(It.IsAny<int>(), CancellationToken.None));
    }
}