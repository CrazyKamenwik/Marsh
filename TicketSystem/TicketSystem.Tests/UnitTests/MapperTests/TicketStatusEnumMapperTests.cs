using AutoMapper;
using FluentAssertions;
using TicketSystem.BLL.Enums;
using TicketSystem.DAL.Entities.Enums;

namespace TicketSystem.Tests.UnitTests.MapperTests;

public class TicketStatusEnumMapperTests
{
    private readonly IMapper _mapper;

    public TicketStatusEnumMapperTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BLL.MapperProfiles.MapperProfile());
            cfg.AddProfile(new API.MapperProfiles.MapperProfile());
        });

        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void FromTicketStatusEnumEntityToTicketStatusEnum_Success()
    {
        // Arrange
        var ticketStatusEnumEntity = TicketStatusEnumEntity.Open;

        // Act
        var result = _mapper.Map<TicketStatusEnumModel>(ticketStatusEnumEntity);

        // Assert
        result.Should().HaveSameNameAs(ticketStatusEnumEntity);
    }

    [Fact]
    public void FromTicketStatusEnumToTicketStatusEnumEntity_Success()
    {
        // Arrange
        var ticketStatusEnum = TicketStatusEnumModel.Open;

        // Act
        var result = _mapper.Map<TicketStatusEnumEntity>(ticketStatusEnum);

        // Assert
        result.Should().HaveSameNameAs(ticketStatusEnum);
    }
}