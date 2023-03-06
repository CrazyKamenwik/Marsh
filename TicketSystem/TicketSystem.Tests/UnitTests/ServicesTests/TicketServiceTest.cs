using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.ServicesTests;

public class TicketServiceTest
{
    private const int TicketId = 1;
    private readonly Mock<IGenericRepository<TicketEntity>> _ticketRepositoryMock;
    private readonly ITicketService _ticketService;
    private readonly Mock<IUserService> _userServiceMock;

    public TicketServiceTest()
    {
        _userServiceMock = new Mock<IUserService>();

        Mock<ILogger<TicketService>> loggerMock = new();

        var mapperMock = MapperMock.GetMapperMock();

        _ticketRepositoryMock = GenericRepositoryMock<TicketEntity>.GetMock(InitializeData.GetAllTicketsEntities());

        _ticketService = new TicketService(_ticketRepositoryMock.Object, mapperMock.Object, _userServiceMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task GetById_CallCorrectMethodOfRepoWithCorrectTypeAndReturnTicket()
    {
        // Act
        var result = await _ticketService.GetTicketById(TicketId, CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _ticketRepositoryMock.Verify(x => x.GetByIdWithInclude(TicketId, CancellationToken.None,
            It.IsAny<Expression<Func<TicketEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task Get_CallCorrectMethodOfRepoWithCorrectTypeAndReturnTickets()
    {
        // Act
        var result = await _ticketService.GetTickets(CancellationToken.None);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<Ticket>>(result);
        Assert.Single(result);
        _ticketRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
            It.IsAny<Func<TicketEntity, bool>?>(),
            It.IsAny<Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>?>(),
            It.IsAny<Expression<Func<TicketEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task Add_CallCorrectMethodOfRepoWithCorrectTypeAndReturnTicket()
    {
        // Arrange
        var ticket = InitializeData.GetTicketModel();

        _userServiceMock.Setup(x => x.GetAvailableOperator(CancellationToken.None))
            .ReturnsAsync(value: null);

        // Act
        var result = await _ticketService.AddTicket(ticket, CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _userServiceMock.Verify(x => x.GetAvailableOperator(CancellationToken.None), Times.Once);
        _ticketRepositoryMock.Verify(x =>
            x.Create(It.IsAny<TicketEntity>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Update_CallCorrectMethodOfRepoWithCorrectTypeAndReturnTicket()
    {
        // Arrange
        var ticket = InitializeData.GetTicketModel();

        // Act
        var result = await _ticketService.UpdateTicket(ticket, CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _ticketRepositoryMock.Verify(x =>
            x.Update(CancellationToken.None, It.IsAny<TicketEntity>()), Times.Once);
    }

    [Fact]
    public async Task Delete_CallCorrectMethodOfRepoWithCorrectType()
    {
        // Act
        await _ticketService.DeleteTicket(TicketId, CancellationToken.None);

        // Assert
        _ticketRepositoryMock.Verify(x => x.Remove(TicketId,
            CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CloseOpenTickets_CallGetCorrectMethodOfRepoWithCorrectTypeAndCloseOneOpenTicket()
    {
        // Arrange
        var ticketsCanBeClosed = InitializeData.GetTicketsThatCanBeClosed();

        _ticketRepositoryMock.Setup(x => x.GetWithInclude(CancellationToken.None,
                It.IsAny<Func<TicketEntity, bool>>(),
                It.IsAny<Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>?>(),
                It.IsAny<Expression<Func<TicketEntity, object>>[]>()))
            .ReturnsAsync(ticketsCanBeClosed);


        // Act
        await _ticketService.CloseOpenTickets(CancellationToken.None);

        // Assert
        _ticketRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
            It.IsAny<Func<TicketEntity, bool>>(),
            It.IsAny<Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>?>(),
            It.IsAny<Expression<Func<TicketEntity, object>>[]>()), Times.Once);
        Assert.DoesNotContain(ticketsCanBeClosed, x => x.TicketStatus == TicketStatusEnumEntity.Open);
    }
}