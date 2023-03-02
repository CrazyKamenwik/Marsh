using Moq;
using TS.MailService.Infrastructure.Abstraction;
using TS.MailService.Infrastructure.Abstraction.Repository;

namespace TS.MailService.Tests.UnitTests.Moq;

public class GenericRepositoryMock<T> where T : IBaseEntity
{
    public static Mock<IGenericRepository<T>> GetMock(IEnumerable<T> entities)
    {
        var genericRepoMock = new Mock<IGenericRepository<T>>();

        genericRepoMock
            .Setup(x => x.Create(entities.First(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        genericRepoMock
            .Setup(x => x.Get(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(entities.First());

        genericRepoMock
            .Setup(x => x.GetAll(CancellationToken.None))
            .ReturnsAsync(entities);

        return genericRepoMock;
    }
}