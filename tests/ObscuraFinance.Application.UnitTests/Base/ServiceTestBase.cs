using AutoMapper;
using Moq;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using ObscuraFinance.Application.UnitTests.Mocks;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class ServiceTestBase
    {
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IMapper> _mapperMock;

        protected ServiceTestBase()
        {
            _unitOfWorkMock = UnitOfWorkMockFactory.Create();
            _mapperMock = new Mock<IMapper>();
        }
    }
}