using System.Linq.Expressions;
using AutoMapper;
using Moq;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.UnitTests.Moq;

namespace TicketSystem.Tests.UnitTests.ServicesTests
{
    public class UserServiceTest
    {
        private readonly Mock<IGenericRepository<UserEntity>> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserService _userService;
        private readonly IEnumerable<User> _users;
        private readonly IEnumerable<UserEntity> _usersEntity;

        public UserServiceTest()
        {
            GetValues(out _usersEntity, out _users);
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = GenericRepositoryMock<UserEntity>.GetMock(_usersEntity);
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnUserAndCorrectMapping()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<User>(_usersEntity.First()))
                .Returns(_users.First());

            // Act
            var result = await _userService.GetUserById(1, CancellationToken.None);

            // Assert
            Assert.IsType<User>(result);
            _mapperMock.Verify(x => x.Map<User>(_usersEntity.First()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetByIdWithInclude(1, CancellationToken.None,
                It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnUsersAndCorrectMapping()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<IEnumerable<User>>(_usersEntity))
                .Returns(_users);

            // Act
            var result = await _userService.GetUsers(CancellationToken.None);

            // Assert
            Assert.IsType<List<User>>(result);
            Assert.Single(result);
            _mapperMock.Verify(x => x.Map<IEnumerable<User>>(_usersEntity), Times.Once);
            _userRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None, null, null,
                It.IsAny<Expression<Func<UserEntity, object>>>()), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnUserAndCorrectMapping()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<User>(_usersEntity.First()))
                .Returns(_users.First());
            _mapperMock.Setup(x => x.Map<UserEntity>(_users.First()))
                .Returns(_usersEntity.First());

            // Act
            var result = await _userService.AddUser(_users.First(), CancellationToken.None);

            // Assert
            Assert.IsType<User>(result);
            _mapperMock.Verify(x => x.Map<User>(_usersEntity.First()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserEntity>(_users.First()), Times.Once);
            _userRepositoryMock.Verify(x => x.Create(_usersEntity.First(), CancellationToken.None)
                , Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnUserAndCorrectMapping()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<User>(_usersEntity.First()))
                .Returns(_users.First());
            _mapperMock.Setup(x => x.Map<UserEntity>(_users.First()))
                .Returns(_usersEntity.First());

            // Act
            var result = await _userService.UpdateUser(_users.First(), CancellationToken.None);

            // Assert
            Assert.IsType<User>(result);
            _mapperMock.Verify(x => x.Map<User>(_usersEntity.First()), Times.Once);
            _mapperMock.Verify(x => x.Map<UserEntity>(_users.First()), Times.Once);
            _userRepositoryMock.Verify(x => x.Update(CancellationToken.None, _usersEntity.First())
                , Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRemoveMethodOfRepository()
        {
            // Arrange

            // Act
            await _userService.DeleteUser(1, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(x => x.Remove(It.IsAny<int>(),
                CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetAvailableOperator_ShouldReturnUserAndCorrectMapping_WhenUserIsExist()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<User>(_usersEntity.First()))
                .Returns(_users.First());

            // Act
            var result = await _userService.GetAvailableOperator(CancellationToken.None);

            // Assert
            Assert.IsType<User>(result);
            _mapperMock.Verify(x => x.Map<User>(_usersEntity.First()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetWithInclude(CancellationToken.None,
                It.IsAny<Func<UserEntity, bool>>(), It.IsAny<Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>?>(),
                It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
        }

        [Fact]
        public async Task GetAvailableOperator_ShouldReturnNull_WhenUserDoesNotExist()
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
                It.IsAny<Func<UserEntity, bool>>(), It.IsAny<Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>?>(),
                It.IsAny<Expression<Func<UserEntity, object>>[]>()), Times.Once);
        }

        private static void GetValues(out IEnumerable<UserEntity> usersEntity, out IEnumerable<User> users)
        {
            var userEntity = new UserEntity
            {
                Id = 1,
                Name = "Vlad",
                UserRole = new UserRoleEntity { Id = 1, Name = "User" }
            };

            var user = new User
            {
                Id = 1,
                Name = "Vlad",
                UserRole = new UserRole() {Id = 1, Name = "User"}
            };

            users = new List<User>() { user };
            usersEntity = new List<UserEntity> { userEntity };
        }
    }
}
