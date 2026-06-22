using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Application.Interfaces;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryResponse>> GetDashboardSummary()
        {
            var response = await _dashboardService.GetDashboardSummaryAsync(CancellationToken.None);

            return Ok(new ApiResponse<DashboardSummaryResponse>
            {
                Success = true,
                Message = "Dashboard summary retrieved successfully",
                Data = response
            });
        }
    }
}
