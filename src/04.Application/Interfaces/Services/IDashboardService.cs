using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;

namespace Obscura.FinanceTracker.Application.Interfaces.Services
{
    /// <summary>
    /// Application service contract for aggregated financial dashboard data.
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Retrieves the dashboard summary: total balances, recent transactions, and expense statistics.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task<DashboardSummaryResponse> GetDashboardSummaryAsync(CancellationToken cancellationToken);
    }
}
