using Moq;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;

namespace ObscuraFinance.Application.UnitTests.Mocks
{

    public static class UnitOfWorkMockFactory
    {
        public static Mock<IUnitOfWork> Create()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            return mock;
        }
    }
}
