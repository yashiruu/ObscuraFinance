using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    /// <summary>
    /// Handles dashboard-related operations, providing summaries and financial insights.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Retrieves the financial dashboard summary: total balances, recent transactions, and expense statistics.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <response code="200">Returns the dashboard summary successfully.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(ApiResponse<DashboardSummaryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DashboardSummaryResponse>> GetDashboardSummary(CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetDashboardSummaryAsync(cancellationToken);

            return Ok(ApiResponse<DashboardSummaryResponse>.SuccessResponse(response, "Dashboard summary retrieved successfully"));
        }
    }
}