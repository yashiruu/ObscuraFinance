using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Interfaces.Services;

namespace Obscura.FinanceTracker.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IDashboardRepository dashboardRepository, ILogger<DashboardService> logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
        }

        public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching dashboard summary");
            return await _dashboardRepository.GetDashboardSummaryAsync();
        }
    }
}
