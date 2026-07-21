using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class DashboardServiceTestBase
    {
        protected readonly Mock<IDashboardRepository> _dashboardRepositoryMock;
        protected readonly Mock<ILogger<DashboardService>> _loggerMock;

        protected readonly DashboardService _dashboardService;

        protected DashboardServiceTestBase()
        {
            _dashboardRepositoryMock = new Mock<IDashboardRepository>();
            _loggerMock = new Mock<ILogger<DashboardService>>();

            _dashboardService = new DashboardService(
                dashboardRepository: _dashboardRepositoryMock.Object,
                logger: _loggerMock.Object
            );
        }
    }
}
