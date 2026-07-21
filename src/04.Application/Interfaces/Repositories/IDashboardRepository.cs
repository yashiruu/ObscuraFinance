using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryResponse> GetDashboardSummaryAsync();
    }
}
