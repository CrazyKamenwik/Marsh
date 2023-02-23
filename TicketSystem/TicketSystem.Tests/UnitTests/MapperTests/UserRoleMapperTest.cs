using AutoMapper;
using FluentAssertions;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;

namespace TicketSystem.Tests.UnitTests.MapperTests;

public class UserRoleMapperTest
{
    private readonly IMapper _mapper;

    public UserRoleMapperTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BLL.MapperProfiles.MapperProfile());
            cfg.AddProfile(new API.MapperProfiles.MapperProfile());
        });

        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void FromUserRoleEntityToTUserRole_Success()
    {
        // Arrange
        var userRoleEntity = new UserRoleEntity
        {
            Id = 1,
            Name = RolesConstants.Operator
        };

        // Act
        var result = _mapper.Map<UserRole>(userRoleEntity);

        // Assert
        result.Should().BeEquivalentTo(userRoleEntity);
    }

    [Fact]
    public void FromUserRoleToTUserRoleEntity_Success()
    {
        // Arrange
        var userRole = new UserRole
        {
            Id = 1,
            Name = RolesConstants.Operator
        };

        // Act
        var result = _mapper.Map<UserRoleEntity>(userRole);

        // Assert
        result.Should().BeEquivalentTo(userRole);
    }
}