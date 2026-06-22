using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    public class DashboardClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/v1/dashboard";

        public DashboardClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardSummaryResponse?> GetSummaryAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/summary");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardSummaryResponse>>();

            return result?.Data;
        }
    }
}
