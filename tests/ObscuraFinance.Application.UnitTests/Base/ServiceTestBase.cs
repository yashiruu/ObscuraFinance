using AutoMapper;
using Moq;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class ServiceTestBase
    {
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock;
        protected readonly Mock<IMapper> MapperMock;

        protected ServiceTestBase()
        {
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            MapperMock = new Mock<IMapper>();
        }
    }
}