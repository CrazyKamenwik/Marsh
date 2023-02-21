using System.Linq.Expressions;
using Moq;
using TicketSystem.DAL.Abstractions;

namespace TicketSystem.Tests.UnitTests.Moq;

internal class GenericRepositoryMock<T> where T : IBaseEntity
{
    public static Mock<IGenericRepository<T>> GetMock(IEnumerable<T> baseEntities)
    {
        var genericRepositoryMock = new Mock<IGenericRepository<T>>();

        genericRepositoryMock
            .Setup(x => x.Create(baseEntities.First(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        genericRepositoryMock
            .Setup(x => x.Update(CancellationToken.None, baseEntities.First()))
            .Returns(Task.CompletedTask);

        genericRepositoryMock
            .Setup(x => x.Remove(It.IsAny<int>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        genericRepositoryMock
            .Setup(x => x.GetByIdWithInclude(It.IsAny<int>(), CancellationToken.None,
                It.IsAny<Expression<Func<T, object>>[]>()))
            .ReturnsAsync(baseEntities.First());

        genericRepositoryMock
            .Setup(x => x.GetWithInclude(CancellationToken.None,
                It.IsAny<Func<T, bool>>(),
                It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
                It.IsAny<Expression<Func<T, object>>[]>()))
            .ReturnsAsync(baseEntities);

        return genericRepositoryMock;
    }
}