using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Enums;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.ServicesTests;

public class TicketServiceTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<TicketEntity>> _ticketRepositoryMock;
    private readonly IEnumerable<Ticket> _tickets;
    private readonly IEnumerable<TicketEntity> _ticketsEntity;
    private readonly ITicketService _ticketService;
    private readonly Mock<IUserService> _userServiceMock;

    public TicketServiceTest()
    {
        GetValues(out _ticketsEntity, out _tickets);
        _userServiceMock = new Mock<IUserService>();
        Mock<ILogger<TicketService>> loggerMock = new();
        _mapperMock = new Mock<IMapper>();
        _ticketRepositoryMock = GenericRepositoryMock<TicketEntity>.GetMock(_ticketsEntity);
        _ticketService = new TicketService(_ticketRepositoryMock.Object, _mapperMock.Object, _userServiceMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnTicketAndCorrectMapping()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<Ticket>(_ticketsEntity.First()))
            .Returns(_tickets.First());

        // Act
        var result = await _ticketService.GetTicketById(1, CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _mapperMock.Verify(x => x.Map<Ticket>(_ticketsEntity.First()), Times.Once);
        _ticketRepositoryMock.Verify(x => x.GetByIdWithInclude(1, CancellationToken.None,
            It.IsAny<Expression<Func<TicketEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task Get_ReturnTicketsAndCorrectMapping()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<IEnumerable<Ticket>>(_ticketsEntity))
            .Returns(_tickets);

        // Act
        var result = await _ticketService.GetTickets(CancellationToken.None);

        // Assert
        Assert.IsType<List<Ticket>>(result);
        Assert.Single(result);
        _mapperMock.Verify(x => x.Map<IEnumerable<Ticket>>(_ticketsEntity), Times.Once);
        _ticketRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
            It.IsAny<Func<TicketEntity, bool>?>(),
            It.IsAny<Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>?>(),
            It.IsAny<Expression<Func<TicketEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task Add_ReturnTicketAndCorrectMapping()
    {
        // Arrange
        _userServiceMock.Setup(x => x.GetAvailableOperator(CancellationToken.None))
            .ReturnsAsync(value: null);
        _mapperMock.Setup(x => x.Map<Ticket>(_ticketsEntity.First()))
            .Returns(_tickets.First());
        _mapperMock.Setup(x => x.Map<TicketEntity>(_tickets.First()))
            .Returns(_ticketsEntity.First());

        // Act
        var result = await _ticketService.AddTicket(_tickets.First(), CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _userServiceMock.Verify(x => x.GetAvailableOperator(CancellationToken.None), Times.Once);
        _mapperMock.Verify(x => x.Map<Ticket>(_ticketsEntity.First()), Times.Once);
        _mapperMock.Verify(x => x.Map<TicketEntity>(_tickets.First()), Times.Once);
        _ticketRepositoryMock.Verify(x => x.Create(_ticketsEntity.First(), CancellationToken.None)
            , Times.Once);
    }

    [Fact]
    public async Task Update_ReturnTicketAndCorrectMapping()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<Ticket>(_ticketsEntity.First()))
            .Returns(_tickets.First());
        _mapperMock.Setup(x => x.Map<TicketEntity>(_tickets.First()))
            .Returns(_ticketsEntity.First());

        // Act
        var result = await _ticketService.UpdateTicket(_tickets.First(), CancellationToken.None);

        // Assert
        Assert.IsType<Ticket>(result);
        _mapperMock.Verify(x => x.Map<Ticket>(_ticketsEntity.First()), Times.Once);
        _mapperMock.Verify(x => x.Map<TicketEntity>(_tickets.First()), Times.Once);
        _ticketRepositoryMock.Verify(x => x.Update(CancellationToken.None, _ticketsEntity.First())
            , Times.Once);
    }

    [Fact]
    public async Task Delete_CallRemoveMethodOfRepository()
    {
        // Arrange

        // Act
        await _ticketService.DeleteTicket(1, CancellationToken.None);

        // Assert
        _ticketRepositoryMock.Verify(x => x.Remove(It.IsAny<int>(),
            CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CloseOpenTickets_CallGetWithIncludeAndCloseOneOpenTicket()
    {
        // Arrange
        var ticketsCanBeClosed = GetTicketsThatCanBeClosed();

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

    private static void GetValues(out IEnumerable<TicketEntity> ticketsEntity, out IEnumerable<Ticket> tickets)
    {
        var user = new User { Id = 1, Name = "Vlad", UserRole = new UserRole { Id = 1, Name = "User" } };
        var ticket = new Ticket(user.Id)
        {
            Id = 1,
            CreatedAt = DateTime.Now,
            Messages = new List<Message>(),
            TicketCreator = user,
            TicketStatus = TicketStatusEnumModel.Open
        };

        var userEntity = new UserEntity
            { Id = 1, Name = "Vlad", UserRole = new UserRoleEntity { Id = 1, Name = "User" } };
        var ticketEntity = new TicketEntity
        {
            Id = 1,
            CreatedAt = DateTime.Now,
            Messages = new List<MessageEntity>(),
            TicketCreator = userEntity,
            TicketCreatorId = userEntity.Id,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        tickets = new List<Ticket> { ticket };
        ticketsEntity = new List<TicketEntity> { ticketEntity };
    }

    private static IEnumerable<TicketEntity> GetTicketsThatCanBeClosed()
    {
        var userEntityVlad = new UserEntity
            { Id = 1, Name = "Vlad", UserRole = new UserRoleEntity { Id = 1, Name = "User" } };

        var userEntityTanya = new UserEntity
            { Id = 2, Name = "Tanya", UserRole = new UserRoleEntity { Id = 1, Name = "User" } };

        var ticketEntityVlad = new TicketEntity
        {
            Id = 1,
            CreatedAt = DateTime.Now - TimeSpan.FromMinutes(70),
            Messages = new List<MessageEntity> { new() { CreatedAt = DateTime.Now - TimeSpan.FromMinutes(61) } },
            TicketCreator = userEntityVlad,
            OperatorId = 1,
            TicketCreatorId = userEntityVlad.Id,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        var ticketEntityTanya = new TicketEntity
        {
            Id = 2,
            CreatedAt = DateTime.Now - TimeSpan.FromMinutes(120),
            Messages = new List<MessageEntity> { new() { CreatedAt = DateTime.Now - TimeSpan.FromMinutes(90) } },
            TicketCreator = userEntityTanya,
            OperatorId = 1,
            TicketCreatorId = userEntityTanya.Id,
            TicketStatus = TicketStatusEnumEntity.Open
        };

        return new List<TicketEntity> { ticketEntityVlad, ticketEntityTanya };
    }
}