using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Shouldly;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.ServicesTests;

public class UserServiceTest
{
    private const int UserId = 1;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<UserEntity>> _userRepositoryMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _mapperMock = MapperMock.GetMapperMock();
        _userRepositoryMock = GenericRepositoryMock<UserEntity>.GetMock(InitializeData.GetAllUsersEntities());
        _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetById_CallCorrectMethodOfRepoWithCorrectTypeAndReturnUser()
    {
        // Act
        var result = await _userService.GetUserById(UserId, CancellationToken.None);

        // Assert
        Assert.IsType<User>(result);
        _userRepositoryMock.Verify(x =>
            x.GetByIdWithInclude(UserId, CancellationToken.None,
                It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task Get_CallCorrectMethodOfRepoWithCorrectTypeAndReturnAllUsers()
    {
        // Act
        var result = await _userService.GetUsers(CancellationToken.None);

        // Assert
        Assert.IsType<List<User>>(result);
        Assert.Equal(2, result.Count());
        _userRepositoryMock.Verify(x =>
            x.GetWithInclude(CancellationToken.None, null, null,
                It.IsAny<Expression<Func<UserEntity, object>>>()), Times.Once);
    }

    [Fact]
    public async Task Add_CallCorrectMethodOfRepoWithCorrectTypeAndReturnUser()
    {
        // Arrange
        var user = InitializeData.GetUserModelUser();

        // Act
        var result = await _userService.AddUser(user, CancellationToken.None);

        // Assert
        Assert.IsType<User>(result);
        result.ShouldBeEquivalentTo(user);
        _userRepositoryMock.Verify(x =>
            x.Create(It.IsAny<UserEntity>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Update_CallCorrectMethodOfRepoWithCorrectTypeAndReturnUser()
    {
        // Arrange
        var userToUpdate = InitializeData.GetUserModelUser();

        // Act
        var result = await _userService.UpdateUser(userToUpdate, CancellationToken.None);

        // Assert
        Assert.IsType<User>(result);
        _userRepositoryMock.Verify(x =>
            x.Update(CancellationToken.None, It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task Delete_CallCorrectMethodOfRepoWithCorrectType()
    {
        // Act
        await _userService.DeleteUser(UserId, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(x =>
            x.Remove(It.IsAny<int>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAvailableOperator_UserIsExist_CallCorrectMethodOfRepoWithCorrectTypeAndReturnUser()
    {
        // Act
        var result = await _userService.GetAvailableOperator(CancellationToken.None);

        // Assert
        Assert.IsType<User>(result);
        _userRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
            It.IsAny<Func<UserEntity, bool>>(),
            It.IsAny<Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>?>(),
            It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
    }

    [Fact]
    public async Task GetAvailableOperator_UserDoesNotExist_CallCorrectMethodOfRepoWithCorrectTypeAndReturnNull()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetWithInclude(CancellationToken.None,
                It.IsAny<Func<UserEntity, bool>>(),
                It.IsAny<Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>>(),
                It.IsAny<Expression<Func<UserEntity, object>>[]>()))
            .ReturnsAsync(Enumerable.Empty<UserEntity>());

        // Act
        var result = await _userService.GetAvailableOperator(CancellationToken.None);

        // Assert
        Assert.Null(result);
        _userRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
            It.IsAny<Func<UserEntity, bool>>(),
            It.IsAny<Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>?>(),
            It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
    }
}