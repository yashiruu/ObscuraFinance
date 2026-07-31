using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Client.Constants;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    /// <summary>
    /// Typed HTTP client for the Dashboard API.
    /// </summary>
    public class DashboardClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardClient"/> class.
        /// </summary>
        public DashboardClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves the financial dashboard summary (balances, recent transactions, expense statistics).
        /// </summary>
        public async Task<DashboardSummaryResponse?> GetSummaryAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Dashboard}/summary");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardSummaryResponse>>();

            return result?.Data;
        }
    }
}
