using AutoMapper;
using FluentAssertions;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;

namespace TicketSystem.Tests.UnitTests.MapperTests;

public class UserMapperTests
{
    private readonly IMapper _mapper;

    public UserMapperTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BLL.MapperProfiles.MapperProfile());
            cfg.AddProfile(new API.MapperProfiles.MapperProfile());
        });

        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void FromShortUserToUser_Success()
    {
        // Arrange
        var shortUser = InitializeData.GetShortUserViewModelUser();
        var expectedUser = new User
        {
            Name = shortUser.Name,
            UserRole = new UserRole
            {
                Name = shortUser.UserRole
            }
        };

        // Act
        var message = _mapper.Map<User>(shortUser);

        // Assert
        message.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void FromUserToUserViewModel_Success()
    {
        // Arrange
        var user = InitializeData.GetUserModelUser();
        var userViewModel = new UserViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            UserRole = user.UserRole.Name
        };

        // Act
        var result = _mapper.Map<UserViewModel>(user);

        // Assert
        result.Should().BeEquivalentTo(userViewModel);
    }

    [Fact]
    public void FromUserEntityToUser_Success()
    {
        // Arrange
        var userEntity = InitializeData.GetUserEntityOperator();

        // Act
        var result = _mapper.Map<User>(userEntity);

        // Assert
        result.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void FromUserToUserEntity_Success()
    {
        // Arrange
        var user = InitializeData.GetUserModelUser();
        var expectedUserEntity = new UserEntity()
        {
            Id = user.Id,
            Name = user.Name,
            Tickets = new List<TicketEntity>(),
            UserRole = new UserRoleEntity()
            {
                Id = user.UserRole.Id,
                Name = user.UserRole.Name
            }
        };

        // Act
        var result = _mapper.Map<UserEntity>(user);

        // Assert
        result.Should().BeEquivalentTo(expectedUserEntity);
    }
}