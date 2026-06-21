using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryResponse> GetDashboardSummaryAsync(CancellationToken cancellationToken);
    }
}
