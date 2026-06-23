using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Client.Constants;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    public class TransactionClient
    {
        private readonly HttpClient _httpClient;

        public TransactionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TransactionListResponse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Transactions);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<TransactionListResponse>>>();

            return result?.Data ?? [];
        }

        public async Task<TransactionDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Transactions}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TransactionDetailResponse>>();

            return result?.Data;
        }

        public async Task CreateAsync(TransactionCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Transactions}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Transactions}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Transactions}/{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Transactions}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
