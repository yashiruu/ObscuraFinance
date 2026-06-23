using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Client.Constants;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    public class DashboardClient
    {
        private readonly HttpClient _httpClient;

        public DashboardClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardSummaryResponse?> GetSummaryAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Dashboard}/summary");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardSummaryResponse>>();

            return result?.Data;
        }
    }
}
