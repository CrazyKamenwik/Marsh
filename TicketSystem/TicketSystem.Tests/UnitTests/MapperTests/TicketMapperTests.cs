using AutoMapper;
using FluentAssertions;
using TicketSystem.API.ViewModels.Tickets;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;

namespace TicketSystem.Tests.UnitTests.MapperTests;

public class TicketMapperTests
{
    private readonly IMapper _mapper;

    public TicketMapperTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BLL.MapperProfiles.MapperProfile());
            cfg.AddProfile(new API.MapperProfiles.MapperProfile());
        });

        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void FromTicketToTicketViewModel_Success()
    {
        // Arrange
        var ticketModelWithoutUsers = InitializeData.GetTicketModel();

        ticketModelWithoutUsers.Operator = InitializeData.GetUserModelOperator();
        ticketModelWithoutUsers.TicketCreator = InitializeData.GetUserModelUser();

        var expectedTicket = InitializeData.GetTicketViewModel();

        // Act
        var result = _mapper.Map<TicketViewModel>(ticketModelWithoutUsers);

        // Assert
        result.Should().BeEquivalentTo(expectedTicket, options => options
            .For(t => t.Messages)
            .Exclude(m => m.User));
    }

    [Fact]
    public void FromTicketEntityToTicket_Success()
    {
        // Arrange
        var ticketEntity = InitializeData.GetTicketEntity();
        var ticket = InitializeData.GetTicketModel();

        // Act
        var result = _mapper.Map<Ticket>(ticketEntity);

        // Assert
        result.Should().BeEquivalentTo(ticket, options => options
            .For(t => t.Messages)
            .Exclude(m => m.CreatedAt)
            .Excluding(t => t.CreatedAt));
    }

    [Fact]
    public void FromTicketToTicketEntity_Success()
    {
        // Arrange
        var ticket = InitializeData.GetTicketModel();
        var ticketEntity = InitializeData.GetTicketEntity();

        // Act
        var result = _mapper.Map<TicketEntity>(ticket);

        // Assert
        result.Should().BeEquivalentTo(ticketEntity, options => options
            .For(t => t.Messages)
            .Exclude(m => m.CreatedAt)
            .Excluding(t => t.CreatedAt));
    }
}