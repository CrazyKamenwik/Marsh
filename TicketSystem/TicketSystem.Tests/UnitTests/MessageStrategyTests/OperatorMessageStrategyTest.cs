using AutoMapper;
using Moq;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.MessagesStrategy;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.MessageStrategyTests
{
    public class OperatorMessageStrategyTest
    {
        private readonly IMessageStrategy _messageStrategy;
        private readonly Mock<IGenericRepository<MessageEntity>> _messageRepositoryMock;
        private readonly Mock<ITicketService> _ticketServiceMock;

        public OperatorMessageStrategyTest()
        {
            var mapperMock = MapperMock.GetMapperMock();

            _ticketServiceMock = new Mock<ITicketService>();
            _messageRepositoryMock = GenericRepositoryMock<MessageEntity>.GetMock(new List<MessageEntity>
                { InitializeData.GetMessageEntityFromUser() });
            _messageStrategy = new OperatorMessageStrategy(_ticketServiceMock.Object,
                _messageRepositoryMock.Object, mapperMock.Object);
        }

        [Theory]
        [InlineData(RolesConstants.Operator, true)]
        [InlineData(RolesConstants.User, false)]
        public void IsApplicable_StringIsOperator_ReturnTrue(string userRole, bool expected)
        {
            // Act
            var result = _messageStrategy.IsApplicable(userRole);

            // Assert
            Assert.Equal(result, expected);
        }

        [Fact]
        public async Task AddMessageAsync_AddMessageToTicketByIdAndReturnMessage()
        {
            // Arrange
            var user = InitializeData.GetUserModelUserWithOpenTicket();
            var message = InitializeData.GetMessageModelFromUser();
            var ticket = InitializeData.GetTicketModel();

            // Act
            var result = await _messageStrategy.AddMessage(message, user, CancellationToken.None);

            // Assert
            Assert.IsType<Message>(result);
            _ticketServiceMock.Verify(x => x.GetTicketById(It.IsAny<int>(), CancellationToken.None));
            _messageRepositoryMock.Verify(x =>
                x.Create(It.IsAny<MessageEntity>(), CancellationToken.None), Times.Once);
        }
    }
}